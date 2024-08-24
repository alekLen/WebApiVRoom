using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface ITagRepository: ISetGetRepository<Tag>
    {
        Task<Tag> GetByName(string name);
        Task<IEnumerable<Tag>> GetAllPaginated(int pageNumber, int pageSize);
    }
}
