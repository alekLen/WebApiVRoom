using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IEmailRepository : ISetGetRepository<Email>
    {
        Task<List<Email>> GetByUser(string clerkId);
        Task<Email> GetByUserPrimary(string clerkId);
    }
}
