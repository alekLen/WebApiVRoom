﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IPinnedVideoService
    {
        Task<PinnedVideoDTO> GetPinnedVideoById(int id);
        Task<PinnedVideoDTO> GetPinnedVideoByChannelId(int channelId);
        Task<PinnedVideoDTO> AddPinnedVideo(PinnedVideoDTO pinnedVideoDTO);
        Task<PinnedVideoDTO> UpdatePinnedVideo(PinnedVideoDTO pinnedVideoDTO);
        Task<PinnedVideoDTO?> GetPinnedVideoOrNullByChannelId(int channelId);
        Task DeletePinnedVideo(int id);
    }
}
