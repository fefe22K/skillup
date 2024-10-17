using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace skillup.Migrations
{
    /// <inheritdoc />
    public partial class cpf1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FuncionarioCursos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "FuncionarioCursos");
        }
    }
}
