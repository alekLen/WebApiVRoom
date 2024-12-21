using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnPinnedVideoToChannelSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PinnedVideoId",
                table: "ChannelSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelSettings_Videos_PinnedVideoId",
                table: "ChannelSettings",
                column: "PinnedVideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelSettings_Videos_PinnedVideoId",
                table: "ChannelSettings");

            migrationBuilder.DropColumn(
                name: "PinnedVideoId",
                table: "ChannelSettings");
        }
    }
}