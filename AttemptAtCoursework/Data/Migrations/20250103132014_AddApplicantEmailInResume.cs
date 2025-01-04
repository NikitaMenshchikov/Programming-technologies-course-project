using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttemptAtCoursework.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicantEmailInResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantMail",
                table: "Resume",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantMail",
                table: "Resume");
        }
    }
}
