using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ISaltService
    {
        Task<SaltDTO> GetSalt(UserDTO u);
        Task AddSalt(SaltDTO s);
    }
}
