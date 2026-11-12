using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoMVC.Migrations
{
    /// <inheritdoc />
    public partial class RestructuringStudentDisciplineFormOfTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FormOfTrainings",
                table: "FormOfTrainings");

            migrationBuilder.DropIndex(
                name: "IX_FormOfTrainings_FormCode",
                table: "FormOfTrainings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disciplines",
                table: "Disciplines");

            migrationBuilder.DropIndex(
                name: "IX_Disciplines_DisciplineCode",
                table: "Disciplines");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FormOfTrainings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Disciplines");

            migrationBuilder.RenameColumn(
                name: "FormCode",
                table: "FormOfTrainings",
                newName: "FormId");

            migrationBuilder.RenameColumn(
                name: "DisciplineCode",
                table: "Disciplines",
                newName: "DisciplineId");

            migrationBuilder.AlterColumn<string>(
                name: "FormOfTrainingId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DisciplineId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormOfTrainings",
                table: "FormOfTrainings",
                column: "FormId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disciplines",
                table: "Disciplines",
                column: "DisciplineId");

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
                principalColumn: "DisciplineId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_FormOfTrainings_FormOfTrainingId",
                table: "Students",
                column: "FormOfTrainingId",
                principalTable: "FormOfTrainings",
                principalColumn: "FormId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormOfTrainings",
                table: "FormOfTrainings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disciplines",
                table: "Disciplines");

            migrationBuilder.RenameColumn(
                name: "FormId",
                table: "FormOfTrainings",
                newName: "FormCode");

            migrationBuilder.RenameColumn(
                name: "DisciplineId",
                table: "Disciplines",
                newName: "DisciplineCode");

            migrationBuilder.AlterColumn<string>(
                name: "FormOfTrainingId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DisciplineId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FormOfTrainings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Disciplines",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormOfTrainings",
                table: "FormOfTrainings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disciplines",
                table: "Disciplines",
                column: "Id");

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
    }
}
