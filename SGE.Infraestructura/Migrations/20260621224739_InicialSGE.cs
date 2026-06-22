using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGE.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class InicialSGE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expedientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Caratula_Valor = table.Column<string>(type: "TEXT", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsuarioUltimoCambio = table.Column<Guid>(type: "TEXT", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expedientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tramites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExpedienteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Etiqueta = table.Column<int>(type: "INTEGER", nullable: false),
                    Contenido_Valor = table.Column<string>(type: "TEXT", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaUltimaModificacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsuarioUltimoCambio = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tramites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tramites_Expedientes_ExpedienteId",
                        column: x => x.ExpedienteId,
                        principalTable: "Expedientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_ExpedienteId",
                table: "Tramites",
                column: "ExpedienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tramites");

            migrationBuilder.DropTable(
                name: "Expedientes");
        }
    }
}
