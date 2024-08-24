using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ICountryService
    {
        Task<CountryDTO> GetCountry(int id);
        Task<IEnumerable<CountryDTO>> GetAllCountries();
        Task<IEnumerable<CountryDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task<CountryDTO> GetByName(string name);
        Task AddCountry(CountryDTO countryDTO);
        Task<CountryDTO> GetCountryByCountryCode(string code);
        Task<CountryDTO> UpdateCountry(CountryDTO countryDTO);
        Task DeleteCountry(int id);
    }
}
