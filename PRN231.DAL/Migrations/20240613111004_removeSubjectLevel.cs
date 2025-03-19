using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN231.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeSubjectLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_SubjectLevels_SubjectLevelId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "SubjectLevels");

            migrationBuilder.RenameColumn(
                name: "SubjectLevelId",
                table: "Bookings",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_SubjectLevelId",
                table: "Bookings",
                newName: "IX_Bookings_SubjectId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Schedules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_LevelId",
                table: "Bookings",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Levels_LevelId",
                table: "Bookings",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Subjects_SubjectId",
                table: "Bookings",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Levels_LevelId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Subjects_SubjectId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_LevelId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "SlotHour",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "SlotMinute",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Bookings",
                newName: "SubjectLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_SubjectId",
                table: "Bookings",
                newName: "IX_Bookings_SubjectLevelId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectLevels_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubjectLevels_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLevels_LevelId",
                table: "SubjectLevels",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectLevels_SubjectId",
                table: "SubjectLevels",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_SubjectLevels_SubjectLevelId",
                table: "Bookings",
                column: "SubjectLevelId",
                principalTable: "SubjectLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
