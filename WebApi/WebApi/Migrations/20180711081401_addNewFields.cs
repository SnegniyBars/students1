using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class addNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaysOfBusy_ScheDays_ScheDayId",
                table: "DaysOfBusy");

            migrationBuilder.AlterColumn<int>(
                name: "ScheDayId",
                table: "DaysOfBusy",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DaysOfBusy_ScheDays_ScheDayId",
                table: "DaysOfBusy",
                column: "ScheDayId",
                principalTable: "ScheDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaysOfBusy_ScheDays_ScheDayId",
                table: "DaysOfBusy");

            migrationBuilder.AlterColumn<int>(
                name: "ScheDayId",
                table: "DaysOfBusy",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_DaysOfBusy_ScheDays_ScheDayId",
                table: "DaysOfBusy",
                column: "ScheDayId",
                principalTable: "ScheDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
