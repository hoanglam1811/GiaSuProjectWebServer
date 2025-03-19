using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN231.DAL.Migrations
{
    /// <inheritdoc />
    public partial class changeSubjecctCredential : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Credentials_SubjectId",
                table: "Credentials");

            migrationBuilder.DropColumn(
                name: "CredentialId",
                table: "Subjects");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_SubjectId",
                table: "Credentials",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Credentials_SubjectId",
                table: "Credentials");

            migrationBuilder.AddColumn<int>(
                name: "CredentialId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_SubjectId",
                table: "Credentials",
                column: "SubjectId",
                unique: true,
                filter: "[SubjectId] IS NOT NULL");
        }
    }
}
