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
        Task<Country> GetByName(string name);
        Task Add(Country country);
        Task Update(Country country);
        Task Delete(int id);
    }
}
