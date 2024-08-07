using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country> GetById(int id);
        Task<IEnumerable<Country>> GetAll();
        Task<Country> GetByName(string name);
        Task<Country> GetByCountryCode(string code);
        Task Add(Country country);
        Task Update(Country country);
        Task Delete(int id);
    }
}
