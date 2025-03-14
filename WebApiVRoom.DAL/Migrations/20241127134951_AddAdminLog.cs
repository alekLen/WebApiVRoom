using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentPosts_Posts_PostId",
                table: "CommentPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentVideos_Videos_VideoId",
                table: "CommentVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryOfBrowsings_Videos_VideoId",
                table: "HistoryOfBrowsings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideo_PlayLists_PlayListId",
                table: "PlayListVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideo_Videos_VideoId",
                table: "PlayListVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Options_OptionId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayListVideo",
                table: "PlayListVideo");

            migrationBuilder.CreateTable(
                name: "AdminLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderUserId = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentReports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayListVideo_PlayListId",
                table: "PlayListVideo",
                column: "PlayListId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentPosts_Posts_PostId",
                table: "CommentPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVideos_Videos_VideoId",
                table: "CommentVideos",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryOfBrowsings_Videos_VideoId",
                table: "HistoryOfBrowsings",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideo_PlayLists_PlayListId",
                table: "PlayListVideo",
                column: "PlayListId",
                principalTable: "PlayLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideo_Videos_VideoId",
                table: "PlayListVideo",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Options_OptionId",
                table: "Votes",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentPosts_Posts_PostId",
                table: "CommentPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentVideos_Videos_VideoId",
                table: "CommentVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoryOfBrowsings_Videos_VideoId",
                table: "HistoryOfBrowsings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideo_PlayLists_PlayListId",
                table: "PlayListVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideo_Videos_VideoId",
                table: "PlayListVideo");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Options_OptionId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "AdminLogs");

            migrationBuilder.DropTable(
                name: "Ads");

            migrationBuilder.DropTable(
                name: "ContentReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayListVideo",
                table: "PlayListVideo");

            migrationBuilder.DropIndex(
                name: "IX_PlayListVideo_PlayListId",
                table: "PlayListVideo");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PlayListVideo",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayListVideo",
                table: "PlayListVideo",
                columns: new[] { "PlayListId", "VideoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentPosts_Posts_PostId",
                table: "CommentPosts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVideos_Videos_VideoId",
                table: "CommentVideos",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryOfBrowsings_Videos_VideoId",
                table: "HistoryOfBrowsings",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideo_PlayLists_PlayListId",
                table: "PlayListVideo",
                column: "PlayListId",
                principalTable: "PlayLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideo_Videos_VideoId",
                table: "PlayListVideo",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Options_OptionId",
                table: "Votes",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Posts_PostId",
                table: "Votes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
