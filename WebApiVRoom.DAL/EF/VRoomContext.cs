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
        public DbSet<CommentVideo> CommentVideos { get; set; }
        public DbSet<AnswerPost> AnswerPosts { get; set; }
        public DbSet<AnswerVideo> AnswerVideos { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<HistoryOfBrowsing> HistoryOfBrowsings { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<LikesDislikesCV> LikesCV { get; set; }
        public DbSet<LikesDislikesCP> LikesCP { get; set; }
        public DbSet<LikesDislikesAV> LikesAV { get; set; }
        public DbSet<LikesDislikesAP> LikesAP { get; set; }
        public DbSet<LikesDislikesV> LikesV { get; set; }
        public DbSet<LikesDislikesP> LikesP { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentPost>()
                    .HasOne(cp => cp.Post)
                    .WithMany(p => p.CommentPosts)
                    .OnDelete(DeleteBehavior.NoAction); // Убираем каскадное удаление

            modelBuilder.Entity<CommentVideo>()
                    .HasOne(cp => cp.Video)
                    .WithMany(p => p.CommentVideos)
                    .OnDelete(DeleteBehavior.NoAction); // Убираем каскадное удаление

            modelBuilder.Entity<HistoryOfBrowsing>()
                   .HasOne(cp => cp.Video)
                   .WithMany(p => p.HistoryOfBrowsings)
                   .OnDelete(DeleteBehavior.NoAction); // Убираем каскадное удаление

            modelBuilder.Entity<PlayListVideo>()
         .HasKey(pv => new { pv.PlayListId, pv.VideoId }); // Первичный ключ составной

            modelBuilder.Entity<PlayListVideo>()
                .HasOne(pv => pv.PlayList)
                .WithMany(p => p.PlayListVideos)
                .HasForeignKey(pv => pv.PlayListId)
                .OnDelete(DeleteBehavior.NoAction); // Отключаем каскадное удаление со стороны PlayList

            modelBuilder.Entity<PlayListVideo>()
                .HasOne(pv => pv.Video)
                .WithMany(v => v.PlayListVideos)
                .HasForeignKey(pv => pv.VideoId)
                .OnDelete(DeleteBehavior.NoAction);


            //modelBuilder.Entity<Post>()
            //   .HasOne(pv => pv.ChannelSettings)
            //   .WithMany(v => v.Posts)
            //   .HasForeignKey(pv => pv.channelSettingsId)
            //   .OnDelete(DeleteBehavior.NoAction);

            //       modelBuilder.Entity<LikesDislikesP>()
            //   .HasOne(ld => ld.Post)
            //   .WithMany() // Связь "многие ко многим" или "один ко многим"
            //   .HasForeignKey(ld => ld.Post.Id) // Указываем внешний ключ
            //   .OnDelete(DeleteBehavior.NoAction);

            //       modelBuilder.Entity<LikesDislikesV>()
            // .HasOne(ld => ld.Video)
            // .WithMany() // Связь "многие ко многим" или "один ко многим"
            // .HasForeignKey(ld => ld.Video.Id) // Указываем внешний ключ
            // .OnDelete(DeleteBehavior.NoAction);

            //       modelBuilder.Entity<LikesDislikesCP>()
            // .HasOne(ld => ld.commentPost)
            // .WithMany() // Связь "многие ко многим" или "один ко многим"
            // .HasForeignKey(ld => ld.commentPost.Id) // Указываем внешний ключ
            // .OnDelete(DeleteBehavior.NoAction);

            //       modelBuilder.Entity<LikesDislikesAP>()
            // .HasOne(ld => ld.answerPost)
            // .WithMany() // Связь "многие ко многим" или "один ко многим"
            // .HasForeignKey(ld => ld.answerPost.Id) // Указываем внешний ключ
            // .OnDelete(DeleteBehavior.NoAction);

            //       modelBuilder.Entity<LikesDislikesCV>()
            // .HasOne(ld => ld.commentVideo)
            // .WithMany() // Связь "многие ко многим" или "один ко многим"
            // .HasForeignKey(ld => ld.commentVideo.Id) // Указываем внешний ключ
            // .OnDelete(DeleteBehavior.NoAction);

            //       modelBuilder.Entity<LikesDislikesAV>()
            //.HasOne(ld => ld.answerVideo)
            //.WithMany() // Связь "многие ко многим" или "один ко многим"
            //.HasForeignKey(ld => ld.answerVideo.Id) // Указываем внешний ключ
            //.OnDelete(DeleteBehavior.NoAction);

        }
    }
}

