using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILanguageRepository
    {
        Task<Language> GetById(int id);
        Task<Language> GetByName(string name);
        Task Add(Language lang);
        Task Update(Language lang);
        Task Delete(int id);
    }
}
