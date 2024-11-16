using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IEmailService
    {
        Task<EmailDTO> GetEmail(int id);
        Task<EmailDTO> GetEmailByUserPrimary(string clerkId);
        Task<IEnumerable<EmailDTO>> GetAllEmailsByUser(string clerkId);
        Task AddEmail(EmailDTO emDTO);
        Task<EmailDTO> UpdateEmail(EmailDTO emDTO);
        Task DeleteEmail(int id);
    }
}
