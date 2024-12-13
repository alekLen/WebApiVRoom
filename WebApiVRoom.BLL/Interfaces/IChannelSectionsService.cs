using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;
using static System.Collections.Specialized.BitVector32;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IChannelSectionsService
    {
        //Task<ChannelSectionsDTO> GetChannelSectionsById(int id);
        Task AddRangeChannelSectionsByClerkId(string clerkId, List<ChannelSectionDTO> t);
        Task UpdateRangeChannelSectionsByClerkId(string clerkId, List<ChannelSectionDTO> t);
        Task<List<ChannelSectionDTO>> GetChannelSectionsAsync(int channelOwnerId);
        Task<List<ChannelSectionDTO>> FindChannelSectionsByChannelOwnerId(string channelOwnerId);
        Task<List<ChannelSectionDTO>> GetChannelSectionsByChannelUrl(string channelUrl);
        Task<List<ChannelSectionDTO>> GetChannelSectionsByChannelNikName(string channelNikname);

        Task<List<ChSectionDTO>> GetAvailableChannelSectionsByChannelOwnerId(string channelOwnerId);



        Task<IEnumerable<ChSectionDTO>> GetAllChSection();
        Task<ChSectionDTO> GetChSectionByTitle(string title);
        Task<ChSectionDTO> GetChSectionById(int id);
        Task<ChSectionDTO> AddChSection(ChSectionDTO ch);
        Task<ChSectionDTO> UpdateChSection(ChSectionDTO ch);
        Task<ChSectionDTO> DeleteChSection(int id);
        Task<bool> IsChSectioneUnique(string title, int chSectionId);


    }
}
