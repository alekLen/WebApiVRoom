using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ILanguageService
    {
        Task<LanguageDTO> GetLanguage(int id);
        Task<IEnumerable<LanguageDTO>> GetAllLanguages();
        Task<IEnumerable<LanguageDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task<LanguageDTO> GetLanguageByName(string name);
        Task AddLanguage(LanguageDTO languageDTO);
        Task<LanguageDTO> UpdateLanguage(LanguageDTO languageDTO);
        Task DeleteLanguage(int id);
    }
}
