﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class HistoryOfBrowsingRepository : IHistoryOfBrowsingRepository
    {
        private readonly VRoomContext db;

        public HistoryOfBrowsingRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task Add(HistoryOfBrowsing history)
        {
            if (history == null)
            {
                throw new ArgumentNullException(nameof(history));
            }

            await db.HistoryOfBrowsings.AddAsync(history);
            await db.SaveChangesAsync();
        }

        public async Task Update(HistoryOfBrowsing history)
        {
            if (history == null)
            {
                throw new ArgumentNullException(nameof(history));
            }

            db.HistoryOfBrowsings.Update(history);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var history = await db.HistoryOfBrowsings.FindAsync(id);
            if (history == null)
            {
                throw new KeyNotFoundException($"HistoryOfBrowsing with ID {id} not found.");
            }

            db.HistoryOfBrowsings.Remove(history);
            await db.SaveChangesAsync();
        }

        public async Task<HistoryOfBrowsing> GetById(int id)
        {
            return await db.HistoryOfBrowsings.FindAsync(id);
        }

        public async Task<IEnumerable<HistoryOfBrowsing>> GetAll()
        {
            return await db.HistoryOfBrowsings.ToListAsync();
        }

        public async Task<IEnumerable<HistoryOfBrowsing>> GetByUserId(int userId)
        {
            return await db.HistoryOfBrowsings
                           .Where(h => h.User.Id == userId)
                           .ToListAsync();
        }

        
    }
}
