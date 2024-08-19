using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ICommentPostService
    {
        Task AddCommentPost(CommentPostDTO commentPostDTO);            
        Task DeleteCommentPost(int id);                                
        Task<CommentPostDTO> GetCommentPost(int id);                  
        Task UpdateCommentPost(CommentPostDTO commentPostDTO);

        Task<List<CommentPostDTO>> GetCommentPostsByPost(int postId);
        Task<List<CommentPostDTO>> GetByPostPaginated(int pageNumber, int pageSize, int postId);
    }
}