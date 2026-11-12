using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class RestructuringDiscipline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FormCode",
                table: "FormOfTrainings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DisciplineCode",
                table: "Disciplines",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_FormOfTrainings_FormCode",
                table: "FormOfTrainings",
                column: "FormCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disciplines_DisciplineCode",
                table: "Disciplines",
                column: "DisciplineCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FormOfTrainings_FormCode",
                table: "FormOfTrainings");

            migrationBuilder.DropIndex(
                name: "IX_Disciplines_DisciplineCode",
                table: "Disciplines");

            migrationBuilder.AlterColumn<string>(
                name: "FormCode",
                table: "FormOfTrainings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DisciplineCode",
                table: "Disciplines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
