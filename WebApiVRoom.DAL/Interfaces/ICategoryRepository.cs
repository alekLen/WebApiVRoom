using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<Category> GetByName(string name);
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(int id);
    }
}
