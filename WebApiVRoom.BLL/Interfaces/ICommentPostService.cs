using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ICommentPostService
    {
        Task<CommentPostDTO> AddCommentPost(CommentPostDTO commentPostDTO);            
        Task<CommentPostDTO> DeleteCommentPost(int id);                                
        Task<CommentPostDTO> GetCommentPost(int id);                  
        Task<CommentPostDTO> UpdateCommentPost(CommentPostDTO commentPostDTO);
        Task<List<CommentPostDTO>> GetByUser(int userId);
        Task<List<CommentPostDTO>> GetByUserPaginated(int pageNumber, int pageSize, int userId);
        Task<List<CommentPostDTO>> GetCommentPostsByPost(int postId);
        Task<List<CommentPostDTO>> GetByPostPaginated(int pageNumber, int pageSize, int postId);
    }
}