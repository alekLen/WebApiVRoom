using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ICommentPostService
    {
        Task AddCommentPost(CommentPostDTO commentPostDTO);            
        Task DeleteCommentPost(int id);                                
        Task<IEnumerable<CommentPostDTO>> GetAllPaginated(int pageNumber, int pageSize); 
        Task<IEnumerable<CommentPostDTO>> GetAllCommentPostsPaginated(int pageNumber, int pageSize); 
        Task<CommentPostDTO> GetCommentPost(int id);                  
        Task UpdateCommentPost(CommentPostDTO commentPostDTO);
    }
}