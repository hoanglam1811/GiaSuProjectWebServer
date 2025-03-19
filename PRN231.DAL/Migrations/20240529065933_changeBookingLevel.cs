using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN231.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeBookingLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Subjects_SubjectId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Bookings",
                newName: "SubjectLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_SubjectId",
                table: "Bookings",
                newName: "IX_Bookings_SubjectLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_SubjectLevels_SubjectLevelId",
                table: "Bookings",
                column: "SubjectLevelId",
                principalTable: "SubjectLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_SubjectLevels_SubjectLevelId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "SubjectLevelId",
                table: "Bookings",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_SubjectLevelId",
                table: "Bookings",
                newName: "IX_Bookings_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Subjects_SubjectId",
                table: "Bookings",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
