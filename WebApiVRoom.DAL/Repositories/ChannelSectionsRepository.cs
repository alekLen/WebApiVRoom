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
    public class ChannelSectionsRepository:IChannelSectionRepository
    {
        private VRoomContext db;
        public ChannelSectionsRepository(VRoomContext context)
        {
            db = context;
        }
        public async Task AddRangeChannelSectionsByClerkId(int channelSettingsId, List<ChannelSection> t)
        {
            try
            {
                if (channelSettingsId == null)
                {
                    throw new ArgumentNullException(nameof(channelSettingsId));
                }

                if (t == null)
                {
                    throw new ArgumentNullException(nameof(t));
                }
                // Привязываем UserId ко всем новым разделам
                t.ForEach(section => section.ChannelSettingsId = channelSettingsId);

                if (t.Any())
                {
                    await db.ChannelSections.AddRangeAsync(t);
                    await db.SaveChangesAsync();
                }

                var existingSections = await db.ChannelSections
                .Where(us => us.ChannelSettingsId == channelSettingsId)
                .ToListAsync();

                db.ChannelSections.UpdateRange(existingSections);
                await db.SaveChangesAsync();
            }
            catch (Exception ex) {  }
        }


        public async Task UpdateRangeChannelSectionsByClerkId(int channelSettingsId, List<ChannelSection> updatedSections)
        {
            if (channelSettingsId == null)
            {
                throw new ArgumentNullException(nameof(channelSettingsId));
            }

            if (updatedSections == null)
            {
                throw new ArgumentNullException(nameof(updatedSections));
            }

            // Получаем существующие записи для данного channelSettingsId
            var existingSections = await db.ChannelSections
                .Include(cs => cs.Channel_Settings)
                .Include(cs => cs.Section)
                .Where(cs => cs.ChannelSettingsId == channelSettingsId)
                .ToListAsync();
            var newSections = new List<ChannelSection>();
            // Синхронизация обновляемых и существующих разделов
            foreach (var updatedSection in updatedSections)
            {
                var existingSection = existingSections.FirstOrDefault(es => es.SectionId == updatedSection.SectionId);
                if (existingSection != null)
                {
                    // Обновляем свойства для существующих записей
                    existingSection.Order = updatedSection.Order;
                    existingSection.IsVisible = updatedSection.IsVisible;
                }
                else
                {
                    // Добавляем новые записи, которых нет в базе
                    newSections.Add(new ChannelSection
                    {
                        ChannelSettingsId = channelSettingsId,
                        SectionId = updatedSection.Section.Id,
                        Order = updatedSection.Order,
                        IsVisible = updatedSection.IsVisible
                    });
                }
            }

            // Удаление разделов, отсутствующих в обновленных данных
            var updatedSectionIds = updatedSections.Select(us => us.SectionId).ToHashSet();
            var sectionsToRemove = existingSections
                .Where(es => !updatedSectionIds.Contains(es.SectionId))
                .ToList();

            foreach (var sectionToRemove in sectionsToRemove)
            {
                sectionToRemove.IsVisible = false;
                sectionToRemove.Order = 0;
            } 
            
            // Обновляем существующие записи
            if (existingSections.Any())
            {
                db.ChannelSections.UpdateRange(existingSections);
            }
            // Добавляем новые записи в базу
            if (newSections.Any())
            {
                await db.ChannelSections.AddRangeAsync(newSections);
            }
            // Применяем изменения
            await db.SaveChangesAsync();
        }
        public async Task<ChannelSection> GetChannelSectionsById(int id)
        {
            return await db.ChannelSections.Include(cp => cp.Channel_Settings).Include(cp=>cp.Section).FirstOrDefaultAsync(ch => ch.Id == id);

        }

        public async Task<IEnumerable<ChannelSection>> GetAllChannelSections()
        {
            return await db.ChannelSections.Include(cp => cp.Channel_Settings).Include(cp => cp.Section).ToListAsync();
        }
        public async Task<List<ChannelSection>> FindChannelSectionsByChannelOwnerId(string channelOwnerId)
        {
            return await db.ChannelSections.Include(cp => cp.Channel_Settings).Include(cp => cp.Section)
                .Where(cs => cs.Channel_Settings.Owner.Clerk_Id == channelOwnerId).ToListAsync();

        }
        public async Task<List<ChannelSection>> GetChannelSectionsAsync(int channelOwnerId)
        {
            return await db.ChannelSections.Include(cp => cp.Channel_Settings).Include(cp => cp.Section)
                .Where(us => us.ChannelSettingsId == channelOwnerId)
                .OrderBy(us => us.Order)
                .ToListAsync();
        }
        public async Task<List<ChannelSection>> GetChannelSectionsByChannelUrl(string channelUrl)
        {
            return await db.ChannelSections.Include(cp => cp.Channel_Settings).Include(cp => cp.Section)
                .Where(cs => cs.Channel_Settings.Channel_URL == channelUrl).ToListAsync();

        }
        public async Task<List<ChannelSection>> GetChannelSectionsByChannelNikName(string channelNikname)
        {
            if (channelNikname == null)
            {
                return await db.ChannelSections.Include(cp => cp.Channel_Settings).Include(cp => cp.Section)
                    .Where(cs => cs.Channel_Settings.ChannelNikName == channelNikname).ToListAsync();
            }
            else
                return null;

        }


        //public Task Delete(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //

        public async Task<IEnumerable<ChSection>> GetAllChSection()
        {
            return await db.ChSections.ToListAsync();
        }

        public async Task<ChSection> GetChSectionById(int id)
        {
            return await db.ChSections.FirstOrDefaultAsync(ch => ch.Id == id);
        }

        public async Task<ChSection> GetChSectionByTitle(string title)
        {
            return await db.ChSections.FirstOrDefaultAsync(ch => ch.Title == title);
        }

        public async Task AddChSection(ChSection ch)
        {
            if (ch == null)
            {
                throw new ArgumentNullException(nameof(ch));
            }

            await db.ChSections.AddAsync(ch);
            await db.SaveChangesAsync();
        }

        public async Task UpdateChSection(ChSection ch)
        {
            if (ch == null)
            {
                throw new ArgumentNullException(nameof(ch));
            }

            db.ChSections.Update(ch);
            await db.SaveChangesAsync();
        }

        public async Task DeleteChSection(int id)
        {
            var u = await db.ChSections.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.ChSections.Remove(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<bool> IsChSectionUnique(string title, int chSectionId)
        {

            return !await db.ChSections.AnyAsync(u => u.Title == title && u.Id != chSectionId);
        }
    }
}
