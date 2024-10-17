using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace skillup.Migrations
{
    /// <inheritdoc />
    public partial class cpf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioCursos_AspNetUsers_FuncionarioId1",
                table: "FuncionarioCursos");

            migrationBuilder.DropIndex(
                name: "IX_FuncionarioCursos_FuncionarioId1",
                table: "FuncionarioCursos");

            migrationBuilder.DropColumn(
                name: "FuncionarioId1",
                table: "FuncionarioCursos");

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId",
                table: "FuncionarioCursos",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuncionarioCursos_FuncionarioId",
                table: "FuncionarioCursos",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioCursos_AspNetUsers_FuncionarioId",
                table: "FuncionarioCursos",
                column: "FuncionarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioCursos_AspNetUsers_FuncionarioId",
                table: "FuncionarioCursos");

            migrationBuilder.DropIndex(
                name: "IX_FuncionarioCursos_FuncionarioId",
                table: "FuncionarioCursos");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "FuncionarioCursos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId1",
                table: "FuncionarioCursos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuncionarioCursos_FuncionarioId1",
                table: "FuncionarioCursos",
                column: "FuncionarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioCursos_AspNetUsers_FuncionarioId1",
                table: "FuncionarioCursos",
                column: "FuncionarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
