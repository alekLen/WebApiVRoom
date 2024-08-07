using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.DAL.EF
{
    public class VRoomContext : DbContext
    {
        public VRoomContext(DbContextOptions<VRoomContext> options)
         : base(options)
        {
            if (Database.EnsureCreated())
            {
               

            }
        }


        public DbSet<User> Users { get; set; }
        public DbSet<ChannelSettings> ChannelSettings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CommentPost> CommentPosts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
    }
}

