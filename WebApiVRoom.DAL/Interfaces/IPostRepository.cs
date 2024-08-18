using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IPostRepository: ISetGetRepository<Post>
    {
        Task<Post> GetByText(string text);
        Task<IEnumerable<Post>> GetByChannelId(int channelSettingsId);
        Task<IEnumerable<Post>> GetByChannelIdPaginated(int pageNumber, int pageSize, int channelSettingsId);
        Task<List<Post>> GetByIds(List<int> ids);
    }
}
