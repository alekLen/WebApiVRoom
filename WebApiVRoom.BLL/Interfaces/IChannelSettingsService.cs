using Microsoft.AspNetCore.Http;
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
        Task<ChannelSettingsDTO> UpdateChannelSettings(ChannelSettingsDTO chSDto, IFormFileCollection channelImg);
        Task<ChannelSettingsDTO> UpdateChannelSettingsShort(ChannelSettingsShortDTO chSDto, IFormFileCollection channelImg);
        Task<ChannelSettingsDTO> DeleteChannelSettings(int id);
        Task<ChannelSettingsDTO> FindByOwner(string ownerId);
        Task<ChannelSettingsDTO> SetLanguageToChannel(string clerkId, string lang);
        Task<ChannelSettingsDTO> GetByUrl(string url);
        Task<ChannelSettingsDTO> GetByNikName(string nik);
        Task<bool> IsNickNameUnique(string nickName, int chSettingsId);
    }
}
