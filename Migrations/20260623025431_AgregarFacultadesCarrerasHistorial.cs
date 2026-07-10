using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaConvenios.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFacultadesCarrerasHistorial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ruc",
                table: "Entidades",
                type: "character varying(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CargoRepresentante",
                table: "Entidades",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CedulaRepresentante",
                table: "Entidades",
                type: "character varying(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoEntidad",
                table: "Entidades",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroResolucion",
                table: "Convenios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Facultades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Siglas = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facultades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    EstadoAnterior = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EstadoNuevo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Observacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCambio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CambiadoPor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstados_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Siglas = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FacultadId = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carreras_Facultades_FacultadId",
                        column: x => x.FacultadId,
                        principalTable: "Facultades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConvenioFacultades",
                columns: table => new
                {
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    FacultadId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvenioFacultades", x => new { x.ConvenioId, x.FacultadId });
                    table.ForeignKey(
                        name: "FK_ConvenioFacultades_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConvenioFacultades_Facultades_FacultadId",
                        column: x => x.FacultadId,
                        principalTable: "Facultades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConvenioCarreras",
                columns: table => new
                {
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    CarreraId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvenioCarreras", x => new { x.ConvenioId, x.CarreraId });
                    table.ForeignKey(
                        name: "FK_ConvenioCarreras_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConvenioCarreras_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Facultades",
                columns: new[] { "Id", "Activo", "Nombre", "Siglas" },
                values: new object[] { 1, true, "Facultad de Ciencias de la Vida y Tecnologías", "FCVT" });

            migrationBuilder.InsertData(
                table: "Carreras",
                columns: new[] { "Id", "Activo", "FacultadId", "Nombre", "Siglas" },
                values: new object[,]
                {
                    { 1, true, 1, "Ingeniería de Software", "IS" },
                    { 2, true, 1, "Ingeniería Ambiental", "IA" },
                    { 3, true, 1, "Ingeniería Agropecuaria y Agroindustria", "IA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carreras_FacultadId",
                table: "Carreras",
                column: "FacultadId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvenioCarreras_CarreraId",
                table: "ConvenioCarreras",
                column: "CarreraId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvenioFacultades_FacultadId",
                table: "ConvenioFacultades",
                column: "FacultadId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstados_ConvenioId",
                table: "HistorialEstados",
                column: "ConvenioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvenioCarreras");

            migrationBuilder.DropTable(
                name: "ConvenioFacultades");

            migrationBuilder.DropTable(
                name: "HistorialEstados");

            migrationBuilder.DropTable(
                name: "Carreras");

            migrationBuilder.DropTable(
                name: "Facultades");

            migrationBuilder.DropColumn(
                name: "CargoRepresentante",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "CedulaRepresentante",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "TipoEntidad",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "NumeroResolucion",
                table: "Convenios");

            migrationBuilder.AlterColumn<string>(
                name: "Ruc",
                table: "Entidades",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13,
                oldNullable: true);
        }
    }
}
