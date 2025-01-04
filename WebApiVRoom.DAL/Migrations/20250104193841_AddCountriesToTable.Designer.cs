﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiVRoom.DAL.EF;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    [DbContext(typeof(VRoomContext))]
    [Migration("20250104193841_AddCountriesToTable")]
    partial class AddCountriesToTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("YourNamespace.PinnedVideo", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<int>("ChannelSettingsId")
                    .HasColumnType("int");

                b.Property<int>("VideoId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("ChannelSettingsId");

                b.HasIndex("VideoId");

                b.ToTable("PinnedVideos");

                b.HasOne("YourNamespace.ChannelSettings", "ChannelSettings")
                    .WithMany()
                    .HasForeignKey("ChannelSettingsId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("YourNamespace.Video", "Video")
                    .WithMany()
                    .HasForeignKey("VideoId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });
#pragma warning restore 612, 618
        }
    }
}