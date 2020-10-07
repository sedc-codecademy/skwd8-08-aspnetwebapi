﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StickyNotes.DataAccess.Entities;
using StickyNotes.DataAccess.Repositories;
using Swashbuckle.AspNetCore.Swagger;
namespace StickyNotes.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Notes API", Version = "v1" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<StickyNotesDatabaseContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString(@"Data Source=(localdb)\MSSQLLocalDB;Database=StickyNotes.Database;Trusted_Connection=True;")));

            services.AddTransient<IRepository<User>, UserRepository>();
            services.AddTransient<IRepository<Note>, NoteRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "To Do List API V1");
                });
            }

            app.UseMvc();
        }
    }
}
