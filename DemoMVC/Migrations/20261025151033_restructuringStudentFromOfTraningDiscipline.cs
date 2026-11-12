using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class restructuringStudentFromOfTraningDiscipline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Disciplines_DisciplineId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_FormOfTrainings_FormOfTrainingId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_DisciplineId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_FormOfTrainingId",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "FormOfTrainingId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DisciplineId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FormOfTrainingId",
                table: "Students",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DisciplineId",
                table: "Students",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DisciplineId",
                table: "Students",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FormOfTrainingId",
                table: "Students",
                column: "FormOfTrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Disciplines_DisciplineId",
                table: "Students",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_FormOfTrainings_FormOfTrainingId",
                table: "Students",
                column: "FormOfTrainingId",
                principalTable: "FormOfTrainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
