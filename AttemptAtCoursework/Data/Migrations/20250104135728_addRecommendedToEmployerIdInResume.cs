using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttemptAtCoursework.Data.Migrations
{
    /// <inheritdoc />
    public partial class addRecommendedToEmployerIdInResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecommendedToEmployerId",
                table: "Resume",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedToEmployerId",
                table: "Resume");
        }
    }
}
