using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag> GetById(int id);
        Task<IEnumerable<Tag>> GetAll();
        Task<Tag> GetByName(string name);
        Task Add(Tag tag);
        Task Update(Tag tag);
        Task Delete(int id);
    }
}
