using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetCar.DB.Migrations
{
    /// <inheritdoc />
    public partial class addWorksForEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorksFor",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorksFor",
                table: "AspNetUsers");
        }
    }
}
