using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ILanguageRepository: ISetGetRepository<Language>
    {
        Task<Language> GetByName(string name);
     
    }
}
