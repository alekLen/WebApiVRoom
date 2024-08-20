﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IPostService
    {
        Task<PostDTO> GetPost(int id);
        Task<List<PostDTO>> GetPostByChannellId(int channelSettingsId);
        Task<List<PostDTO>> GetPostByChannellIdPaginated(int pageNumber, int pageSize, int channelSettingsId);
        Task<IEnumerable<PostDTO>> GetAllPosts();
        Task<PostDTO> GetPostByText(string text);
        Task AddPost(PostDTO postDTO);
        Task UpdatePost(PostDTO postDTO);
        Task DeletePost(int id);

    }
}