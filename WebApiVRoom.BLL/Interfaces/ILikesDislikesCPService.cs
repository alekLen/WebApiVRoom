using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ILikesDislikesCPService
    {
        Task<LikesDislikesCPDTO> Add(LikesDislikesCPDTO t);
        Task<LikesDislikesCPDTO> Get(int commentId, string userid);
    }
}
