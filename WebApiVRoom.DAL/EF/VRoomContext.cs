using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
           
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<PinnedVideo> PinnedVideos { get; set; }
        public DbSet<ChannelSettings> ChannelSettings { get; set; }
        public DbSet<ChannelSection> ChannelSections { get; set; }
        public DbSet<ChSection> ChSections { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CommentPost> CommentPosts { get; set; }
        public DbSet<CommentVideo> CommentVideos { get; set; }
        public DbSet<AnswerPost> AnswerPosts { get; set; }
        public DbSet<AnswerVideo> AnswerVideos { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<HistoryOfBrowsing> HistoryOfBrowsings { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<PlayListVideo> PlayListVideo { get; set; }
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
        public DbSet<OptionsForPost> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<WebRTCSession> WebRTCSessions { get; set; }
        public DbSet<WebRTCConnection> WebRTCConnections { get; set; }
        public DbSet<VideoView> VideoViews { get; set; }
        public DbSet<ContentReport> ContentReports { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentPost>()
                    .HasOne(cp => cp.Post)
                    .WithMany(p => p.CommentPosts)
                    .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<CommentVideo>()
                    .HasOne(cp => cp.Video)
                    .WithMany(p => p.CommentVideos)
                    .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<HistoryOfBrowsing>()
                   .HasOne(cp => cp.Video)
                   .WithMany(p => p.HistoryOfBrowsings)
                   .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<PlayListVideo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();

                entity.HasOne(d => d.PlayList)
                    .WithMany(p => p.PlayListVideo)
                    .HasForeignKey(d => d.PlayListId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Video)
                    .WithMany(v => v.PlayListVideos)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Vote>()
                .HasOne(pv => pv.Post)
                .WithMany(v => v.Voutes)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vote>()
               .HasOne(pv => pv.Option)
               .WithMany(v => v.Voutes)
               .OnDelete(DeleteBehavior.NoAction);


        }
    }
}