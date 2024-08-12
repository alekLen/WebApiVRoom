﻿using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPlayListRepository: ISetGetRepository<PlayList>
    {
        Task<PlayList> GetByUser(int userId);
    }
}