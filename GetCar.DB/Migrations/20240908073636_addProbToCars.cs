using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GetCar.DB.Migrations
{
    /// <inheritdoc />
    public partial class addProbToCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ABSBrakes",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AirConditionServices",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Airbag",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Audioinput",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Bluetooth",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CDplayer",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationPolicy",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cruisecontrol",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EBDbrakes",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Electricmirrors",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Foglights",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GPS",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NavigationSystemServices",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Power",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProtectionDescription",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProtectionTitle",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Remotecontrol",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Roofbox",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sensors",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToddlerSeatServices",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "USBInput",
                table: "Cars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ABSBrakes",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "AirConditionServices",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Airbag",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Audioinput",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Bluetooth",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CDplayer",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CancellationPolicy",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Cruisecontrol",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "EBDbrakes",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Electricmirrors",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Foglights",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "GPS",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "NavigationSystemServices",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ProtectionDescription",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ProtectionTitle",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Remotecontrol",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Roofbox",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Sensors",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ToddlerSeatServices",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "USBInput",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BookingNumber",
                table: "Bookings");
        }
    }
}
