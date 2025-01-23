using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public class PostRepository : IPostRepository
    {
        private VRoomContext db;
        public PostRepository(VRoomContext context)
        {
            this.db = context;
        }

        public async Task Add(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }
            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var u = await db.Posts.FindAsync(id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                await DeleteAllDependencies(u);
                db.Posts.Remove(u);
                await db.SaveChangesAsync();
            }
        }

        async Task DeleteAllDependencies(Post u)
        {
            var comments = await db.CommentPosts.Where(m => m.Post == u).ToListAsync();

            foreach (var comment in comments)
            {
                var answs = await db.AnswerPosts.Where(m => m.CommentPost_Id == comment.Id).ToListAsync();
                foreach (var answer in answs)
                {
                    var likes = await db.LikesAP.Where(m=>m.answerPost==answer).ToListAsync();
                    if (likes != null)
                        db.LikesAP.RemoveRange(likes);
                   
                    db.AnswerPosts.Remove(answer);
                }
                var likes2 = await db.LikesCP.Where(m => m.commentPost==comment).ToListAsync();
                if (likes2 != null)
                   db.LikesCP.RemoveRange(likes2);
               
                db.CommentPosts.Remove(comment);
            }

            var likes3 = await db.LikesP.Where(m => m.Post==u).ToListAsync();
            if (likes3 != null)
                db.LikesP.RemoveRange(likes3);
            
            var votes = await db.Votes.Where(m => m.Post ==u).ToListAsync();
            if(votes != null)
               db.Votes.RemoveRange(votes);

            var options = await db.Options.Where(m => m.Post == u).ToListAsync();
            if(options != null)
               db.Options.RemoveRange(options);

        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            return await db.Posts.Include(m => m.ChannelSettings).Include(m => m.Options).ToListAsync();
        }

       
        public async Task<Post> GetById(int id)
        {
            return await db.Posts.Include(m => m.ChannelSettings).Include(m => m.Options).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Post> GetByText(string text)
        {
            return await db.Posts.Include(m => m.ChannelSettings).Include(m => m.Options).FirstOrDefaultAsync(m => m.Text == text);
        }

        public async Task<IEnumerable<Post>> GetByChannellId(int channelSettingsId)
        {
            return await db.Posts.Include(m => m.ChannelSettings).Include(m => m.Options).Where(m => m.ChannelSettings.Id == channelSettingsId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetByChannellIdPaginated(int pageNumber, int pageSize, int channelSettingsId)
        {
            return await db.Posts
                .Include(m => m.ChannelSettings)
                .Include(m => m.Options)
                .Where(m => m.ChannelSettings.Id == channelSettingsId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task Update(Post post)
        {
            var u = await db.Posts.FindAsync(post.Id);
            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }
            else
            {
                db.Posts.Update(u);
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<Post>> GetByIds(List<int> ids)
        {
            return await db.Posts
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }
    }
}
