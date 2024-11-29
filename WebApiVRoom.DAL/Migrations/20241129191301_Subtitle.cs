using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Subtitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subtitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoId = table.Column<int>(type: "int", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PuthToFile = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtitles_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subtitles_VideoId",
                table: "Subtitles",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subtitles");
        }
    }
}
