using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttemptAtCoursework.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteEmployerIdInCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Company");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployerId",
                table: "Company",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
