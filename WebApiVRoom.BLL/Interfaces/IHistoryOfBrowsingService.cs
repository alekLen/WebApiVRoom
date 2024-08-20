﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface IHistoryOfBrowsingService
    {
        Task<HistoryOfBrowsingDTO> GetById(int id);
        Task<HistoryOfBrowsingDTO> Add(HistoryOfBrowsingDTO t);
        Task<HistoryOfBrowsingDTO> Update(HistoryOfBrowsingDTO t);
        Task<HistoryOfBrowsingDTO> Delete(int id);
        Task<List<HistoryOfBrowsingDTO>> GetByUserId(int userId);
        Task<List<HistoryOfBrowsingDTO>> GetByUserIdPaginated(int pageNumber, int pageSize, int userId);
    }
}