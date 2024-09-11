using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetCar.DB.Migrations
{
    /// <inheritdoc />
    public partial class addDailyAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DailyAvailability",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyAvailability",
                table: "Cars");
        }
    }
}
