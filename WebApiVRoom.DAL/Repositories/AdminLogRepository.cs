using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Interfaces;

namespace WebApiVRoom.DAL.Repositories
{
    public class AdminLogRepository : IAdminLogRepository
    {
        private VRoomContext db;
        public AdminLogRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<AdminLog>> GetPaginatedAndSorted(int page, int perPage)
        {
            return await db.AdminLogs.OrderByDescending(x => x.Date).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }

        public async Task Add(AdminLog adminLog)
        {
            db.AdminLogs.Add(adminLog);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            AdminLog adminLog = await db.AdminLogs.FindAsync(id);
            if (adminLog != null)
                db.AdminLogs.Remove(adminLog);
            await db.SaveChangesAsync();
        }

        public async Task<AdminLog> Get(int id)
        {
            return await db.AdminLogs.FindAsync(id);
        }

        public async Task Update(AdminLog adminLog)
        {
            db.AdminLogs.Update(adminLog);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AdminLog>> GetPaginatedAndSortedWithQuery(int page, int perPage, string type, string? searchQuery)
        {
            return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Action == type || x.Description.Contains(searchQuery)).OrderByDescending(x => x.Date).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }

        public async Task<IEnumerable<AdminLog>> GetPaginated(int page, int perPage)
        {
            return await db.AdminLogs.Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }

        public async Task<IEnumerable<AdminLog>> GetPaginatedWithQuery(int page, int perPage, string? searchQuery)
        {
            return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
        }

        public async Task<IEnumerable<AdminLog>> GetAll()
        {
            return await db.AdminLogs.ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return await db.AdminLogs.CountAsync();
        }

        public async Task<int> GetCountWithQuery(string? searchQuery)
        {
            return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).CountAsync();
        }

        public async Task<IEnumerable<AdminLog>> GetPaginatedAndSortedWithQuery(int page, int perPage, string? searchQuery, string? sortField, string? sortOrder)
        {
            if (sortField == "Action")
            {
                if (sortOrder == "asc")
                {
                    return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).OrderBy(x => x.Action).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                }
                else
                {
                    return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).OrderByDescending(x => x.Action).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                }
            }
            else if (sortField == "Description")
            {
                if (sortOrder == "asc")
                {
                    return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).OrderBy(x => x.Description).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                }
                else
                {
                    return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).OrderByDescending(x => x.Description).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                }
            }
            else if (sortField == "Date")
            {
                if (sortOrder == "asc")
                {
                    return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).OrderBy(x => x.Date).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                }
                else
                {
                    return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).OrderByDescending(x => x.Date).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
                }
            }
            else
            {
                return await db.AdminLogs.Where(x => x.Action.Contains(searchQuery) || x.Description.Contains(searchQuery)).OrderByDescending(x => x.Date).Skip((page - 1) * perPage).Take(perPage).ToListAsync();
            }
        }
    }
}
