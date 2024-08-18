using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICountryRepository: ISetGetRepository<Country>
    {
        Task<Country> GetByName(string name);
        Task<Country> GetByCountryCode(string code);
        Task<IEnumerable<Country>> GetAllPaginated(int pageNumber, int pageSize);
    }
}
