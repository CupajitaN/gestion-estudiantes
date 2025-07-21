using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class RelacionEstudianteUsuarioNueva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Estudiantes_EstudianteId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_EstudianteId",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Estudiantes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_UsuarioId",
                table: "Estudiantes",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_Usuarios_UsuarioId",
                table: "Estudiantes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_Usuarios_UsuarioId",
                table: "Estudiantes");

            migrationBuilder.DropIndex(
                name: "IX_Estudiantes_UsuarioId",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Estudiantes");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EstudianteId",
                table: "Usuarios",
                column: "EstudianteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Estudiantes_EstudianteId",
                table: "Usuarios",
                column: "EstudianteId",
                principalTable: "Estudiantes",
                principalColumn: "Id");
        }
    }
}
