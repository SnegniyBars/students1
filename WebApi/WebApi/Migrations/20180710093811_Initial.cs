using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeetingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheDays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheDays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DaysOfBusy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoomId = table.Column<int>(nullable: true),
                    TimeOfBusy = table.Column<TimeSpan>(nullable: false),
                    TimeOfFree = table.Column<TimeSpan>(nullable: false),
                    Holder = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CurrentWeek = table.Column<bool>(nullable: false),
                    CurrentDay = table.Column<bool>(nullable: false),
                    ScheDayId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysOfBusy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaysOfBusy_MeetingRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "MeetingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DaysOfBusy_ScheDays_ScheDayId",
                        column: x => x.ScheDayId,
                        principalTable: "ScheDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DaysOfBusy_RoomId",
                table: "DaysOfBusy",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_DaysOfBusy_ScheDayId",
                table: "DaysOfBusy",
                column: "ScheDayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaysOfBusy");

            migrationBuilder.DropTable(
                name: "MeetingRooms");

            migrationBuilder.DropTable(
                name: "ScheDays");
        }
    }
}
