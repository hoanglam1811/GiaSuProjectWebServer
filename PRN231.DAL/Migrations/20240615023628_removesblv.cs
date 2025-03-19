using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN231.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removesblv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BookingUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BookingUsers");
        }
    }
}
