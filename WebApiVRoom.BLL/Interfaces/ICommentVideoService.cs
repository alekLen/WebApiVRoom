using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ICommentVideoService
    {
        Task<IEnumerable<CommentVideoDTO>> GetAllCommentVideos();
        Task<CommentVideoDTO> GetCommentVideoById(int id);
        Task<CommentVideoDTO> GetCommentVideoByVideo(int videoId);
        Task<IEnumerable<CommentVideoDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task AddCommentVideo(CommentVideoDTO commentVideoDTO);
        Task UpdateCommentVideo(CommentVideoDTO commentVideoDTO);
        Task DeleteCommentVideo(int id);
    }
}