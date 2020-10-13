﻿using Models;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface INotesService
    {
        IEnumerable<NoteDtoGetAll> GetAll();
        Note Get(Guid id);
        Note Add(NoteDtoAdd model);
        Note Edit(NoteDtoEdit model);
        bool Delete(Guid id);
    }
}
