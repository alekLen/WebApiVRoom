using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPlayListVideoRepository
    {
        Task<IEnumerable<PlayListVideo>> GetAll();
        Task<PlayListVideo> GetById(int id);
        Task Add(PlayListVideo t);
        Task Delete(int id);
        Task Update(PlayListVideo t);
        Task<IEnumerable<PlayListVideo>> GetByPlayListIdAsync(int playListId);
    }
}
