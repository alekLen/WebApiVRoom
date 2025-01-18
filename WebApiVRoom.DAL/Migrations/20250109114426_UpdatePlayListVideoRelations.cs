using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlayListVideoRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideos_PlayLists_PlayListId",
                table: "PlayListVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideos_Videos_VideoId",
                table: "PlayListVideos");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideos_PlayLists_PlayListId",
                table: "PlayListVideos",
                column: "PlayListId",
                principalTable: "PlayLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideos_Videos_VideoId",
                table: "PlayListVideos",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideos_PlayLists_PlayListId",
                table: "PlayListVideos");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayListVideos_Videos_VideoId",
                table: "PlayListVideos");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideos_PlayLists_PlayListId",
                table: "PlayListVideos",
                column: "PlayListId",
                principalTable: "PlayLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListVideos_Videos_VideoId",
                table: "PlayListVideos",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
