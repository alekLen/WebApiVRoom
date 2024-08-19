using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ICategoryRepository: ISetGetRepository<Category>
    {
        Task<Category> GetByName(string name);
        Task<IEnumerable<Category>> GetAllPaginated(int pageNumber, int pageSize);
    }
}
