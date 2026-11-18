using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DalilAcademy.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Capacity_Range",
                table: "Courses",
                sql: "\"Capacity\" >= 15 AND \"Capacity\" <= 9999");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Capacity_Range",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Courses");
        }
    }
}
