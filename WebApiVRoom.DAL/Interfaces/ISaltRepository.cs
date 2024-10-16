﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ISaltRepository
    {
        Task<Salt> Get(User u);
        Task AddItem(Salt s);
    }
}
