using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.Interfaces
{
    public interface IChannelSectionRepository
    {
        Task AddRangeChannelSectionsByClerkId(int channelSettingsId, List<ChannelSection> t);
        Task UpdateRangeChannelSectionsByClerkId(int channelSettingsId, List<ChannelSection> t);
        Task<List<ChannelSection>> GetChannelSectionsAsync(int channelOwnerId);
        Task<List<ChannelSection>> FindChannelSectionsByChannelOwnerId(string channelOwnerId);
        Task<List<ChannelSection>> GetChannelSectionsByChannelUrl(string channelUrl);
        Task<List<ChannelSection>> GetChannelSectionsByChannelNikName(string channelNikname);
        Task<IEnumerable<ChSection>> GetAllChSection();
        Task<ChSection> GetChSectionById(int id);
        Task<ChSection> GetChSectionByTitle(string title);
        Task AddChSection(ChSection ch);
        Task UpdateChSection(ChSection ch);
        Task DeleteChSection(int id); 
        Task<bool> IsChSectionUnique(string title, int chSectionId);
    }
}
