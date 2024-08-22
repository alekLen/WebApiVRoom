using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ICommentVideoService
    {
        Task<List<CommentVideoDTO>> GetAllCommentVideos();
        Task<CommentVideoDTO> GetCommentVideoById(int id);
        Task<List<CommentVideoDTO>> GetCommentsVideoByVideo(int videoId);
        Task<List<CommentVideoDTO>> GetByVideoPaginated(int pageNumber, int pageSize,int videoId);
        Task<CommentVideoDTO> AddCommentVideo(CommentVideoDTO commentVideoDTO);
        Task<CommentVideoDTO> UpdateCommentVideo(CommentVideoDTO commentVideoDTO);
        Task<CommentVideoDTO> DeleteCommentVideo(int id);     
        Task<List<CommentVideoDTO>> GetByUser(int userId);
        Task<List<CommentVideoDTO>> GetByUserPaginated(int pageNumber, int pageSize, int userId);
 
    }
}