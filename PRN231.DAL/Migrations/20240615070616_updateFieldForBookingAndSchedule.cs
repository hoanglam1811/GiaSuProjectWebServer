using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN231.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldForBookingAndSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SlotHour",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SlotMinute",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Schedules",
                newName: "DurationInSeconds");

            migrationBuilder.AddColumn<int>(
                name: "NumOfSlots",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerSlot",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfSlots",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PricePerSlot",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "DurationInSeconds",
                table: "Schedules",
                newName: "ServiceId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Schedules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Schedules",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SlotHour",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlotMinute",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
