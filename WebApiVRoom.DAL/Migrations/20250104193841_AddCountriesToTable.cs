using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiVRoom.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCountriesToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Countries",
               columns: new[] { "Id", "Name", "CountryCode" },
         values: new object[,]
 {
     { 1, "NotSelected", "NotSelected" },
     { 2, "UnitedStates", "US" },
     { 3, "Canada", "CA" },
     { 4, "Ukraine", "UA" },
     { 5, "UnitedKingdom", "UK" },
     { 6, "Germany", "DE" },
     { 7, "France", "FR" },
     { 8, "Spain", "ES" },
     { 9, "Italy", "IT" },
     { 10, "Russia", "RU" },
     { 11, "China", "CN" },
     { 12, "India", "IN" },
     { 13, "Australia", "AU" },
     { 14, "Brazil", "BR" },
     { 15, "Mexico", "MX" },
     { 16, "Japan", "JP" },
     { 17, "SouthKorea", "KR" },
     { 18, "SouthAfrica", "ZA" },
     { 19, "Argentina", "AR" },
     { 20, "Sweden", "SE" },
     { 21, "Norway", "NO" },
     { 22, "Denmark", "DK" }

 });

            migrationBuilder.InsertData(
                table: "ChSections",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    {10, "ForYou" },
                    {11, "HighRaitingVideos" },
                 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //не обязательно
            //   migrationBuilder.DeleteData(
            //table: "Countries",
            //keyColumn: "Id",
            //keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });
        }
    }
}