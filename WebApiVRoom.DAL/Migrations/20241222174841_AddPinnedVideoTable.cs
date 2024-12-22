using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPinnedVideoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "PinnedVideos",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ChannelSettingsId = table.Column<int>(nullable: false),
                VideoId = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PinnedVideos", x => x.Id);
                table.ForeignKey(
                    name: "FK_PinnedVideos_ChannelSettings",
                    column: x => x.ChannelSettingsId,
                    principalTable: "ChannelSettings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_PinnedVideos_Videos",
                    column: x => x.VideoId,
                    principalTable: "Videos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

            migrationBuilder.CreateIndex(
                name: "IX_PinnedVideos_ChannelSettingsId",
                table: "PinnedVideos",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PinnedVideos_VideoId",
                table: "PinnedVideos",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PinnedVideos");
        }
    }
}