using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddChannelSectionAndChSectionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelSettingsId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    IsVisible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelSections_ChannelSettings_ChannelSettingsId",
                        column: x => x.ChannelSettingsId,
                        principalTable: "ChannelSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelSections_ChSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "ChSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelSections_ChannelSettingsId",
                table: "ChannelSections",
                column: "ChannelSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelSections_SectionId",
                table: "ChannelSections",
                column: "SectionId");

            migrationBuilder.InsertData(
                table: "ChSections",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "home" },
                    {2,  "Video" },
                    { 3,"shorts" },
                    {4,"Broadcasts" },
                    {5, "playlists" },
                    { 6,"subscriptionsSection" },
                    {7, "PinnedVideoSection" },
                    {8, "posts" },
                    {9, "About" },
                 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ChannelSections");
            migrationBuilder.DropTable(name: "ChSections");
        }
    }


}
