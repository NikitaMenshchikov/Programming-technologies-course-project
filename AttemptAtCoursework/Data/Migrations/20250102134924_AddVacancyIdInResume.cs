using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttemptAtCoursework.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVacancyIdInResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "VacancyId",
                table: "Resume",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VacancyId",
                table: "Resume");
        }
    }
}
