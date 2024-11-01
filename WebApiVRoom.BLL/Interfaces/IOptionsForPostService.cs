using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IOptionsForPostService
    {
        Task AddOptionsForPost(OptionsForPostDTO vDTO);
        Task DeleteOptions(int id);
    }
}
