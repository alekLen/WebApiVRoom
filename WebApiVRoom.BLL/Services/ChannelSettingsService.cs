using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class ChannelSettingsService
    {
        IUnitOfWork Database { get; set; }

        public ChannelSettingsService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<ChannelSettingsDTO> GetChannelSettings(int id)
        {
            var u = await Database.ChannelSettings.GetById(id);
            if (u == null)
                return null;
            return ChannelSettingsToChannelSettingsDTO(u);
        }
        public ChannelSettingsDTO ChannelSettingsToChannelSettingsDTO(ChannelSettings ch)
        {
            return new ChannelSettingsDTO
            {
                Id = ch.Id,
                ChannelName = ch.ChannelName,
                DateJoined = ch.DateJoined,
                Description = ch.Description,
                ChannelBanner = ch.ChannelBanner,
                Owner_Id = ch.Owner.Id,
                Language_Id = ch.Language.Id,
                Country_Id = ch.Country.Id,
                Notification = ch.Notification 
            };
        }
        public ChannelSettings ChannelSettingsDTOToChannelSettings(ChannelSettingsDTO chDto, ChannelSettings ch)
        {
            ch.Id = chDto.Id;
            ch.ChannelName = chDto.ChannelName;
            ch.DateJoined = chDto.DateJoined;
            ch.Description = chDto.Description;
            ch.ChannelBanner = chDto.ChannelBanner;
            ch.Owner.Id = chDto.Owner_Id;
            ch.Language.Id = chDto.Language_Id;
            ch.Country.Id = chDto.Country_Id;
            ch.Notification = chDto.Notification;

            return ch;
        }

        public async Task<ChannelSettingsDTO> UpdateChannelSettings(ChannelSettingsDTO chDto)
        {
            ChannelSettings ch = await Database.ChannelSettings.GetById(chDto.Id);
            if (ch == null)
                throw new KeyNotFoundException("Video not found");
            ChannelSettings chUpd = ChannelSettingsDTOToChannelSettings(chDto, ch);
            await Database.ChannelSettings.Update(chUpd);

            ChannelSettingsDTO chDto2 = ChannelSettingsToChannelSettingsDTO(chUpd);
            return chDto2;
        }

        public async Task<ChannelSettingsDTO> DeleteChannelSettings(int id)
        {
            ChannelSettings ch = await Database.ChannelSettings.GetById(id);
            if (ch == null)
                throw new KeyNotFoundException("Video not found");

            await Database.ChannelSettings.Delete(id);

            ChannelSettingsDTO chDto = ChannelSettingsToChannelSettingsDTO(ch);
            return chDto;
        }
    }
}
