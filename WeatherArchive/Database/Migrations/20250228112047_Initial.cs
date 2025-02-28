using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WeatherArchive.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    DewPoint = table.Column<double>(type: "float", nullable: false),
                    Pressure = table.Column<int>(type: "int", nullable: false),
                    WindSpeed = table.Column<int>(type: "int", nullable: true),
                    Cloudiness = table.Column<int>(type: "int", nullable: true),
                    LowerCloudCover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HorizontalVisibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phenomena = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WindDirections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WindDirections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportWindDirections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: true),
                    WindDirectionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportWindDirections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportWindDirections_WeatherReports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "WeatherReports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportWindDirections_WindDirections_WindDirectionId",
                        column: x => x.WindDirectionId,
                        principalTable: "WindDirections",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "WindDirections",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "С" },
                    { 2, "Ю" },
                    { 3, "З" },
                    { 4, "В" },
                    { 5, "СЗ" },
                    { 6, "СВ" },
                    { 7, "ЮЗ" },
                    { 8, "ЮВ" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportWindDirections_ReportId",
                table: "ReportWindDirections",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportWindDirections_WindDirectionId",
                table: "ReportWindDirections",
                column: "WindDirectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportWindDirections");

            migrationBuilder.DropTable(
                name: "WeatherReports");

            migrationBuilder.DropTable(
                name: "WindDirections");
        }
    }
}
