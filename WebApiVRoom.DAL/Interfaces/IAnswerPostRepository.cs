using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IAnswerPostRepository : ISetGetRepository<AnswerPost>
    {
        Task<AnswerPost> GetByComment(int comId);
        Task<AnswerPost> GetByUser(int userId);
        Task<AnswerPost> GetByDate(DateTime date);
    }
}
