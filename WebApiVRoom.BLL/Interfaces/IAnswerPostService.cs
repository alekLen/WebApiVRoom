using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IAnswerPostService
    {
        Task<AnswerPostDTO> GetById(int id);
        Task<AnswerPostDTO> Add(AnswerPostDTO t);
        Task<AnswerPostDTO> Update(AnswerPostDTO t);
        Task<AnswerPostDTO> Delete(int id);
        Task<AnswerPostDTO> GetByComment(int comId);
        Task<AnswerPostDTO> GetByUser(int userId);
    }
}
