using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetCar.DB.Migrations
{
    /// <inheritdoc />
    public partial class FeedbackAndCarRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CarId",
                table: "Feedbacks",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Cars_CarId",
                table: "Feedbacks",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "CarID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Cars_CarId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_CarId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Feedbacks");
        }
    }
}
