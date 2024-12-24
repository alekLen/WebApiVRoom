using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Infrastructure;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class PinnedVideoService : IPinnedVideoService
    {
        IUnitOfWork Database { get; set; }

        public PinnedVideoService(IUnitOfWork database)
        {
            Database = database;
        }


        public async Task AddPinnedVideo(PinnedVideoDTO pinnedVideoDTO)
        {
            try
            {
                var chSet = await Database.ChannelSettings.GetById(pinnedVideoDTO.ChannelSettingsId);
                var video = await Database.Videos.GetById(pinnedVideoDTO.VideoId);

                if (chSet == null)
                    throw new ValidationException("Wrong channel!", "");

                if (video == null)
                    throw new ValidationException("Wrong video!", "");

                PinnedVideo pinnedVideo = new PinnedVideo();

                pinnedVideo.Channel_Settings = chSet;
                pinnedVideo.ChannelSettingsId = chSet.Id;

                pinnedVideo.Video = video;
                pinnedVideo.VideoId = video.Id;

                await Database.PinnedVideos.Add(pinnedVideo);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<PinnedVideoDTO> UpdatePinnedVideo(PinnedVideoDTO pinnedVideoDTO)
        {
            try
            {
                PinnedVideo pinnedVideo = await Database.PinnedVideos.GetById(pinnedVideoDTO.Id);

                if (pinnedVideo == null)
                    throw new ValidationException("Wrong pinned video!", "");


                var chSet = await Database.ChannelSettings.GetById(pinnedVideoDTO.ChannelSettingsId);
                var video = await Database.Videos.GetById(pinnedVideoDTO.VideoId);

                if (chSet == null)
                    throw new ValidationException("Wrong channel!", "");

                if (video == null)
                    throw new ValidationException("Wrong video!", "");

                pinnedVideo.Channel_Settings = chSet;
                pinnedVideo.ChannelSettingsId = chSet.Id;

                pinnedVideo.Video = video;
                pinnedVideo.VideoId = video.Id;

                await Database.PinnedVideos.Update(pinnedVideo);



                return pinnedVideoDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeletePinnedVideo(int id)
        {
            try
            {
                await Database.PinnedVideos.Delete(id);

            }
            catch { }
        }



        public async Task<PinnedVideoDTO> GetPinnedVideoById(int id)
        {
            var a = await Database.PinnedVideos.GetById(id);

            if (a == null)
                throw new ValidationException("Wrong tag!", "");

            PinnedVideoDTO pinnedVideoDTO = new PinnedVideoDTO();
            pinnedVideoDTO.Id = a.Id;

            pinnedVideoDTO.ChannelSettingsId = a.Channel_Settings.Id;
            pinnedVideoDTO.VideoId = a.Video.Id;

            return pinnedVideoDTO;
        }

        public async Task<PinnedVideoDTO> GetPinnedVideoByChannelId(int channelId)
        {
            var a = await Database.PinnedVideos.GetPinnedVideoByChannelId(channelId);

            if (a == null)
                throw new ValidationException("Wrong tag!", "");

            PinnedVideoDTO pinnedVideoDTO = new PinnedVideoDTO();
            pinnedVideoDTO.Id = a.Id;

            pinnedVideoDTO.ChannelSettingsId = a.Channel_Settings.Id;
            pinnedVideoDTO.VideoId = a.Video.Id;

            return pinnedVideoDTO;
        }

    }
}
