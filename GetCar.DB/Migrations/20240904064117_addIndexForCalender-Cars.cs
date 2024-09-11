using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetCar.DB.Migrations
{
    /// <inheritdoc />
    public partial class addIndexForCalenderCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PickupLocation",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Car_PickupLocation",
                table: "Cars",
                column: "PickupLocation");

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_Date",
                table: "Calendars",
                column: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Car_PickupLocation",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Calendar_Date",
                table: "Calendars");

            migrationBuilder.AlterColumn<string>(
                name: "PickupLocation",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
