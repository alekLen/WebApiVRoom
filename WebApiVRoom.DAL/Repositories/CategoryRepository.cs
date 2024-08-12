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
    public class CategoryRepository : ICategoryRepository
    {
        private VRoomContext db;

        public CategoryRepository(VRoomContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await db.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await db.Categories.FirstOrDefaultAsync(m => m.Id == id); 
        }

        public async Task<Category> GetByName(string name)
        {
            return await db.Categories.FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task Add(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();
        }

        public async Task Update(Category category)
        {
            var u = await db.Categories.FindAsync(category.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Categories.Update(u);
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(int Id)
        {
            var u = await db.Categories.FindAsync(Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Categories.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Category>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.Categories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
