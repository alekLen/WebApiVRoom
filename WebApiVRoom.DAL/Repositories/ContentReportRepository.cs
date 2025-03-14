using Microsoft.EntityFrameworkCore;
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
    public class ContentReportRepository : IContentReportRepository
    {
        private VRoomContext db;
        public ContentReportRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<ContentReport>> GetPaginated(int page, int perPage, string? searchQuery)
        {
            if (page <= 0) page = 1;
            if (perPage <= 0) perPage = 10;
            
            bool isNumber = int.TryParse(searchQuery, out int idSearch);

            return await db.ContentReports
                .Where(x => 
                    string.IsNullOrEmpty(searchQuery) || 
                    x.Title.Contains(searchQuery) || 
                    (isNumber && x.Id == idSearch) || 
                    x.Description.Contains(searchQuery)
                )
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync();
        }

        public async Task<ContentReport> Get(int id)
        {
            return await db.ContentReports.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Add(ContentReport contentReport)
        {
            if (contentReport == null)
            {
                throw new ArgumentNullException(nameof(contentReport));
            }

            await db.ContentReports.AddAsync(contentReport);
            await db.SaveChangesAsync();
        }

        public async Task Update(ContentReport contentReport)
        {
            if (contentReport == null)
            {
                throw new ArgumentNullException(nameof(contentReport));
            }

            db.ContentReports.Update(contentReport);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var contentReport = await db.ContentReports.FirstOrDefaultAsync(x => x.Id == id);
            if (contentReport == null)
            {
                throw new ArgumentNullException(nameof(contentReport));
            }

            db.ContentReports.Remove(contentReport);
            await db.SaveChangesAsync();
        }

        public async Task<int> Count(string? searchQuery)
        {
            bool isNumber = int.TryParse(searchQuery, out int idSearch);
            
            return await db.ContentReports
                .Where(x =>
                        string.IsNullOrEmpty(searchQuery) ||  
                        x.Title.Contains(searchQuery) ||    
                        (isNumber && x.Id == idSearch) ||   
                        x.Description.Contains(searchQuery) 
                )
                .CountAsync();
        }


        public async Task<IEnumerable<ContentReport>> GetAll()
        {
            return await db.ContentReports.ToListAsync();
        }

        public async Task<IEnumerable<ContentReport>> GetByUserIdPaginated(int userId, int page, int pageSize)
        {
            return await db.ContentReports
                .Where(x => x.SenderUserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
