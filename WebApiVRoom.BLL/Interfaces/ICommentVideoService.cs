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
        Task AddCommentVideo(CommentVideoDTO commentVideoDTO);
        Task UpdateCommentVideo(CommentVideoDTO commentVideoDTO);
        Task DeleteCommentVideo(int id);


    }
}