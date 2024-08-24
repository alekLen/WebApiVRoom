using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IChannelSettingsService
    {
        Task<ChannelSettingsDTO> GetChannelSettings(int id);
        Task<ChannelSettingsDTO> UpdateChannelSettings(ChannelSettingsDTO chDto);
        Task<ChannelSettingsDTO> DeleteChannelSettings(int id);
        Task<ChannelSettingsDTO> FindByOwner(string ownerId);
        Task<ChannelSettingsDTO> SetLanguageToChannel(string clerkId, string lang);
    }
}
