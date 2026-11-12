using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class RestructuringAgainStudentDisciplineFormOfTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_FormOfTrainings_FormOfTrainingId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "FormOfTrainingId",
                table: "Students",
                newName: "FormId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_FormOfTrainingId",
                table: "Students",
                newName: "IX_Students_FormId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_FormOfTrainings_FormId",
                table: "Students",
                column: "FormId",
                principalTable: "FormOfTrainings",
                principalColumn: "FormId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_FormOfTrainings_FormId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "FormId",
                table: "Students",
                newName: "FormOfTrainingId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_FormId",
                table: "Students",
                newName: "IX_Students_FormOfTrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_FormOfTrainings_FormOfTrainingId",
                table: "Students",
                column: "FormOfTrainingId",
                principalTable: "FormOfTrainings",
                principalColumn: "FormId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
