using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IAnswerVideoService
    {
        Task<AnswerVideoDTO> GetById(int id);
        Task<AnswerVideoDTO> Add(AnswerVideoDTO t);
        Task<AnswerVideoDTO> Update(AnswerVideoDTO t);
        Task<AnswerVideoDTO> Delete(int id);
        Task<IEnumerable<AnswerVideoDTO>> GetByComment(int comId);
        Task<AnswerVideoDTO> GetByUser(int userId);
    }
}
