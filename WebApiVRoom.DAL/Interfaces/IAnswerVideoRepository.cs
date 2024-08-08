using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IAnswerVideoRepository
    {
        Task<AnswerVideo> GetById(int id);
        Task<AnswerVideo> GetByComment(int comId);
        Task<AnswerVideo> GetByUser(int userId);
        Task<AnswerVideo> GetByDate(DateTime date);
        Task Add(AnswerVideo answerVideo);
        Task Update(AnswerVideo answerVideo);
        Task Delete(int Id);
    }
}
