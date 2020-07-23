using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VLM.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 30, nullable: false),
                    LastName = table.Column<string>(maxLength: 30, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MoviesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 60, nullable: false),
                    Director = table.Column<string>(maxLength: 30, nullable: false),
                    Language = table.Column<string>(maxLength: 30, nullable: false),
                    Genre = table.Column<string>(maxLength: 30, nullable: false),
                    ReleaseYear = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    ReturnDays = table.Column<int>(nullable: false),
                    Fine = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MoviesId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 25, nullable: false),
                    FirstName = table.Column<string>(maxLength: 30, nullable: false),
                    LastName = table.Column<string>(maxLength: 30, nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<long>(nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    RecordsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    MoviesId = table.Column<int>(nullable: false),
                    TakenDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    IsCleared = table.Column<bool>(nullable: true),
                    Fine = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.RecordsId);
                    table.ForeignKey(
                        name: "FK_Records_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "MoviesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Records_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Admin",
                columns: new[] { "AdminId", "Email", "FirstName", "LastName", "Password" },
                values: new object[] { 15031999, "surisagar@gmail.com", "Sagar", "Suri", "Sagarsuri" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "MoviesId", "Description", "Director", "Fine", "Genre", "Language", "ReleaseYear", "ReturnDays", "Title" },
                values: new object[,]
                {
                    { 1, "When an unexpected enemy emerges that threatens the fate of mankind, Nick Fury, Director of S.H.I.E.L.D., finds himself in need of a team to pull the world back from the brink of disaster. Spanning the globe, a daring recruitment effort begins.", "Josan", 400, "Thriller", "English", 2012, 20, "Avengers" },
                    { 2, "Tony Stark builds an artificial intelligence system named Ultron with the help of Bruce Banner. And when things go wrong, it's up to Earth's mightiest heroes to stop the villainous Ultron from enacting his terrible plan.", "Josan", 700, "Thriller", "English", 2012, 25, "Avengers: Age Of Ultron" },
                    { 3, "With the powerful Thanos on the verge of raining destruction upon the universe, the Avengers and their Superhero allies risk everything in the ultimate showdown of all time.", "Marvels", 800, "Action", "Hindi", 2018, 20, "Avengers: Infinity War" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "FirstName", "IsActive", "LastName", "Password", "Phone", "UserName" },
                values: new object[] { 1, new DateTime(1999, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "surisagar900@gmail.com", "Sagar", true, "Suri", "Sagarsuri", 9876543210L, "surisagar900" });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordsId", "Fine", "IsCleared", "MoviesId", "ReturnDate", "TakenDate", "UserId" },
                values: new object[] { 1, 0, null, 1, new DateTime(2020, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordsId", "Fine", "IsCleared", "MoviesId", "ReturnDate", "TakenDate", "UserId" },
                values: new object[] { 2, 0, null, 3, new DateTime(2020, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Records_MoviesId",
                table: "Records",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserId",
                table: "Records",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
