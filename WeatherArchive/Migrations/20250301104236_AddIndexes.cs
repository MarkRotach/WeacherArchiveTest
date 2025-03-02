using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherArchive.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WeatherReports_Date_Time",
                table: "WeatherReports",
                columns: new[] { "Date", "Time" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WeatherReports_Date_Time",
                table: "WeatherReports");
        }
    }
}
