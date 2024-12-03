﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApiVRoom.DAL.Entities;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Broadcasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BroadcastId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StreamId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Broadcasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clerk_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelSettings_Id = table.Column<int>(type: "int", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnMySubscriptionChannelActivity = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnActivityOnMyChannel = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnRecomendedVideo = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnOnActivityOnMyComments = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnOthersMentionOnMyChannel = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnShareMyContent = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnPromotionalContent = table.Column<bool>(type: "bit", nullable: false),
                    SubscribedOnMainEmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribedOnMySubscriptionChannelActivity = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribedOnActivityOnMyChannel = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribedOnRecomendedVideo = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribedOnOnActivityOnMyComments = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribedOnOthersMentionOnMyChannel = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribedOnShareMyContent = table.Column<bool>(type: "bit", nullable: false),
                    EmailSubscribedOnPromotionalContent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelBanner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelPlofilePhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Channel_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelNikName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Notification = table.Column<bool>(type: "bit", nullable: false),
                    SubscriptionCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelSettings_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelSettings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelSettings_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AnswerPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clerkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CommentPost_Id = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerPosts_ChannelSettings_UserId",
                        column: x => x.UserId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AnswerVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clerkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CommentVideo_Id = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerVideos_ChannelSettings_UserId",
                        column: x => x.UserId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberId = table.Column<int>(type: "int", nullable: true),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: false),
                    Tittle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VRoomVideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    IsShort = table.Column<bool>(type: "bit", nullable: false),
                    Cover = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Visibility = table.Column<bool>(type: "bit", nullable: false),
                    IsAgeRestriction = table.Column<bool>(type: "bit", nullable: false),
                    IsCopyright = table.Column<bool>(type: "bit", nullable: false),
                    Audience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastViewedPosition = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LikesAP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesAP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesAP_AnswerPosts_answerPostId",
                        column: x => x.answerPostId,
                        principalTable: "AnswerPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LikesAV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerVideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesAV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesAV_AnswerVideos_answerVideoId",
                        column: x => x.answerVideoId,
                        principalTable: "AnswerVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CommentPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clerkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentPosts_ChannelSettings_UserId",
                        column: x => x.UserId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CommentPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LikesP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesP_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Options_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CategoryVideo",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    VideosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryVideo", x => new { x.CategoriesId, x.VideosId });
                    table.ForeignKey(
                        name: "FK_CategoryVideo_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryVideo_Videos_VideosId",
                        column: x => x.VideosId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CommentVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clerkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentVideos_ChannelSettings_UserId",
                        column: x => x.UserId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CommentVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HistoryOfBrowsings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeCode = table.Column<int>(type: "int", nullable: false),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryOfBrowsings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryOfBrowsings_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoryOfBrowsings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HistoryOfBrowsings_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LikesV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    likeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    like = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesV_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlayLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Access = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChannelSettingsId = table.Column<int>(type: "int", nullable: true),
                    VideoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayLists_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlayLists_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TagVideo",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    VideosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                     table.PrimaryKey("PK_TagVideo", x => new { x.TagsId, x.VideosId });
                    table.ForeignKey(
                        name: "FK_TagVideo_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TagVideo_Videos_VideosId",
                        column: x => x.VideosId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LikesCP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    commentPostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesCP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesCP_CommentPosts_commentPostId",
                        column: x => x.commentPostId,
                        principalTable: "CommentPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Votes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LikesCV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    commentVideoId = table.Column<int>(type: "int", nullable: false),
                    CommentPostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikesCV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikesCV_CommentPosts_CommentPostId",
                        column: x => x.CommentPostId,
                        principalTable: "CommentPosts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LikesCV_CommentVideos_commentVideoId",
                        column: x => x.commentVideoId,
                        principalTable: "CommentVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlayListVideo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayListId = table.Column<int>(type: "int", nullable: false),
                    VideoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayListVideo_PlayLists_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayListVideo_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerPosts_UserId",
                table: "AnswerPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerVideos_UserId",
                table: "AnswerVideos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryVideo_VideosId",
                table: "CategoryVideo",
                column: "VideosId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelSettings_CountryId",
                table: "ChannelSettings",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelSettings_LanguageId",
                table: "ChannelSettings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelSettings_OwnerId",
                table: "ChannelSettings",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPosts_PostId",
                table: "CommentPosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPosts_UserId",
                table: "CommentPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVideos_UserId",
                table: "CommentVideos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVideos_VideoId",
                table: "CommentVideos",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_UserId",
                table: "Emails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOfBrowsings_ChannelSettingsId",
                table: "HistoryOfBrowsings",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOfBrowsings_UserId",
                table: "HistoryOfBrowsings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryOfBrowsings_VideoId",
                table: "HistoryOfBrowsings",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesAP_answerPostId",
                table: "LikesAP",
                column: "answerPostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesAV_answerVideoId",
                table: "LikesAV",
                column: "answerVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesCP_commentPostId",
                table: "LikesCP",
                column: "commentPostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesCV_CommentPostId",
                table: "LikesCV",
                column: "CommentPostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesCV_commentVideoId",
                table: "LikesCV",
                column: "commentVideoId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesP_PostId",
                table: "LikesP",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_LikesV_VideoId",
                table: "LikesV",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ChannelSettingsId",
                table: "Notifications",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_PostId",
                table: "Options",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayLists_ChannelSettingsId",
                table: "PlayLists",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayLists_UserId",
                table: "PlayLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayLists_VideoId",
                table: "PlayLists",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayListVideo_VideoId",
                table: "PlayListVideo",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ChannelSettingsId",
                table: "Posts",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ChannelSettingsId",
                table: "Subscriptions",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubscriberId",
                table: "Subscriptions",
                column: "SubscriberId");

            migrationBuilder.CreateIndex(
                name: "IX_TagVideo_VideosId",
                table: "TagVideo",
                column: "VideosId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ChannelSettingsId",
                table: "Videos",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_OptionId",
                table: "Votes",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PostId",
                table: "Votes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "music" },
                    {2,  "trending" },
                    { 3,"news" },
                    {4,"games" },
                    {5, "sport" },
                    { 6,"films" },
                    {7, "education" },

                 });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "Music" },
                    { "Video" },
                    { "Comedy" },
                    { "Science" },
                    { "History" },
                    { "Wild_animals" },
                    { "Travel" },
                    { "Nature" },
                    { "Films" },
                    { "Summer" },
                    { "News" },
                    { "Cooking" },
                    { "Good_weather" },
                    { "Pets" },
                    { "Sport" },
                    { "Dreams" },
                    { "Beautiful_place" },
                    { "Education" },

                 });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Name" },
                values: new object[,]
                {
                    { "music" },
                    { "trending" },
                    { "news" },
                    {"live" },
                    { "sport" },
                    { "films" },
                    { "games" },
                    { "education" },

                 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Broadcasts");

            migrationBuilder.DropTable(
                name: "CategoryVideo");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "HistoryOfBrowsings");

            migrationBuilder.DropTable(
                name: "LikesAP");

            migrationBuilder.DropTable(
                name: "LikesAV");

            migrationBuilder.DropTable(
                name: "LikesCP");

            migrationBuilder.DropTable(
                name: "LikesCV");

            migrationBuilder.DropTable(
                name: "LikesP");

            migrationBuilder.DropTable(
                name: "LikesV");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PlayListVideo");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "TagVideo");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AnswerPosts");

            migrationBuilder.DropTable(
                name: "AnswerVideos");

            migrationBuilder.DropTable(
                name: "CommentPosts");

            migrationBuilder.DropTable(
                name: "CommentVideos");

            migrationBuilder.DropTable(
                name: "PlayLists");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "ChannelSettings");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}