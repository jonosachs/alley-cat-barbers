using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlleyCatBarbers.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookingTimeSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeSlot",
                table: "Bookings",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "Bookings");
        }
    }
}
