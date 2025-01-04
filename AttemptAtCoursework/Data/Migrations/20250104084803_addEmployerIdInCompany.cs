using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttemptAtCoursework.Data.Migrations
{
    /// <inheritdoc />
    public partial class addEmployerIdInCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployerId",
                table: "Company",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Company");
        }
    }
}
