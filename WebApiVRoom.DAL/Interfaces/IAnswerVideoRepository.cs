using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IAnswerVideoRepository: ISetGetRepository<AnswerVideo>
    {
        Task<AnswerVideo> GetByComment(int comId);
        Task<AnswerVideo> GetByUser(int userId);
        Task<AnswerVideo> GetByDate(DateTime date);
        Task<List<AnswerVideo>> GetByIds(List<int> ids);
    }
}
