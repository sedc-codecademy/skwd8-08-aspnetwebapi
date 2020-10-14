﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SEDC.NoteApp.DataAccess;
using SEDC.NoteApp.DataModels;
using SEDC.NoteApp.Models;
using SEDC.NoteApp.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SEDC.NoteApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserDTO> _userRepository;
        private readonly IOptions<AppSettings> _options;
        public UserService(IRepository<UserDTO> userRepository, IOptions<AppSettings> options)
        {
            _userRepository = userRepository;
            _options = options;
        }

        public UserModel Authenticate(string username, string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            var hashedPassword = Encoding.ASCII.GetString(md5data);

            var user = _userRepository.GetAll()
                .SingleOrDefault(x => x.Username == username && x.Password == hashedPassword);

            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userModel = new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            return userModel;

        }

        public void Register(RegisterModel model)
        {
            //Validations
            if (string.IsNullOrEmpty(model.FirstName))
                throw new Exception("First name is required");

            if(string.IsNullOrEmpty(model.LastName))
                throw new Exception("Last name is required");

            if (string.IsNullOrEmpty(model.UserName))
                throw new Exception("Username is required");

            if (!ValidUsername(model.UserName))
                throw new Exception("Username already in use");

            if (string.IsNullOrEmpty(model.Password))
                throw new Exception("Password is required");

            if (model.Password != model.ConfirmPassword)
                throw new Exception("Passwords did not match");

            if(!ValidPassword(model.Password))
                throw new Exception("Password is too week");


            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(Encoding.ASCII.GetBytes(model.Password));
            var hasshedPassword = Encoding.ASCII.GetString(md5data);

            var user = new UserDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.UserName,
                Password = hasshedPassword
            };

            _userRepository.Add(user);
        }

        private static bool ValidPassword(string password)
        {
            var passwordRegex = new Regex("^(?=.*[0-9])(?=.*[a-z]).{6,20}$");
            var match = passwordRegex.Match(password);
            return match.Success;
        }

        private bool ValidUsername(string username)
        {
            return _userRepository.GetAll().All(x => x.Username != username);
        }
    }
}
