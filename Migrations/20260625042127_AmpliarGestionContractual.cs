using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaConvenios.Migrations
{
    /// <inheritdoc />
    public partial class AmpliarGestionContractual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CondicionesRenovacion",
                table: "Convenios",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConfidencialidadIndefinida",
                table: "Convenios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DomicilioContractual",
                table: "Convenios",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSuscripcion",
                table: "Convenios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaUltimaFirma",
                table: "Convenios",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuenteFinanciamiento",
                table: "Convenios",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MecanismoSolucionControversias",
                table: "Convenios",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Moneda",
                table: "Convenios",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Presupuesto",
                table: "Convenios",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneErogacion",
                table: "Convenios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ActividadesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HorasPlanificadas = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Lugar = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Responsable = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActividadesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AmbitosConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmbitosConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmbitosConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CierresConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Causal = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Resumen = table.Column<string>(type: "text", nullable: false),
                    ObligacionesPendientes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ResponsablesSeguimiento = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CierresConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CierresConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClausulasConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClausulasConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClausulasConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConveniosRelacionados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    ConvenioRelacionadoId = table.Column<int>(type: "integer", nullable: false),
                    TipoRelacion = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConveniosRelacionados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConveniosRelacionados_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConveniosRelacionados_Convenios_ConvenioRelacionadoId",
                        column: x => x.ConvenioRelacionadoId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EstudiantesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    CarreraId = table.Column<int>(type: "integer", nullable: true),
                    Identificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NumeroPoliza = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CubiertoSeguro = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudiantesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstudiantesConvenio_Carreras_CarreraId",
                        column: x => x.CarreraId,
                        principalTable: "Carreras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EstudiantesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvaluacionesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Calificacion = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Conclusion = table.Column<string>(type: "text", nullable: false),
                    Responsable = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluacionesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluacionesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModificacionesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModificacionesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModificacionesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObligacionesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    Actor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FechaCumplimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Evidencia = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObligacionesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObligacionesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    EntidadId = table.Column<int>(type: "integer", nullable: true),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Alias = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TipoParte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Principal = table.Column<bool>(type: "boolean", nullable: false),
                    Direccion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Ruc = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartesConvenio_Entidades_EntidadId",
                        column: x => x.EntidadId,
                        principalTable: "Entidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ParticipacionesActividad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActividadConvenioId = table.Column<int>(type: "integer", nullable: false),
                    EstudianteConvenioId = table.Column<int>(type: "integer", nullable: false),
                    HorasAsignadas = table.Column<int>(type: "integer", nullable: false),
                    HorasCumplidas = table.Column<int>(type: "integer", nullable: false),
                    Calificacion = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    Evaluacion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CertificadoEmitido = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCertificado = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipacionesActividad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipacionesActividad_ActividadesConvenio_ActividadConve~",
                        column: x => x.ActividadConvenioId,
                        principalTable: "ActividadesConvenio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipacionesActividad_EstudiantesConvenio_EstudianteConv~",
                        column: x => x.EstudianteConvenioId,
                        principalTable: "EstudiantesConvenio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FirmantesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    ParteConvenioId = table.Column<int>(type: "integer", nullable: true),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cargo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Identificacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TipoFirma = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    FechaFirma = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FundamentoRepresentacion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmantesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirmantesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FirmantesConvenio_PartesConvenio_ParteConvenioId",
                        column: x => x.ParteConvenioId,
                        principalTable: "PartesConvenio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ResponsablesConvenio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConvenioId = table.Column<int>(type: "integer", nullable: false),
                    ParteConvenioId = table.Column<int>(type: "integer", nullable: true),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Rol = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cargo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Principal = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsablesConvenio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponsablesConvenio_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponsablesConvenio_PartesConvenio_ParteConvenioId",
                        column: x => x.ParteConvenioId,
                        principalTable: "PartesConvenio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesConvenio_ConvenioId",
                table: "ActividadesConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_AmbitosConvenio_ConvenioId_Nombre",
                table: "AmbitosConvenio",
                columns: new[] { "ConvenioId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CierresConvenio_ConvenioId",
                table: "CierresConvenio",
                column: "ConvenioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClausulasConvenio_ConvenioId",
                table: "ClausulasConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_ConveniosRelacionados_ConvenioId_ConvenioRelacionadoId",
                table: "ConveniosRelacionados",
                columns: new[] { "ConvenioId", "ConvenioRelacionadoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConveniosRelacionados_ConvenioRelacionadoId",
                table: "ConveniosRelacionados",
                column: "ConvenioRelacionadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesConvenio_CarreraId",
                table: "EstudiantesConvenio",
                column: "CarreraId");

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesConvenio_ConvenioId_Identificacion",
                table: "EstudiantesConvenio",
                columns: new[] { "ConvenioId", "Identificacion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesConvenio_ConvenioId",
                table: "EvaluacionesConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmantesConvenio_ConvenioId",
                table: "FirmantesConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmantesConvenio_ParteConvenioId",
                table: "FirmantesConvenio",
                column: "ParteConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_ModificacionesConvenio_ConvenioId",
                table: "ModificacionesConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_ObligacionesConvenio_ConvenioId",
                table: "ObligacionesConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_PartesConvenio_ConvenioId",
                table: "PartesConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_PartesConvenio_EntidadId",
                table: "PartesConvenio",
                column: "EntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipacionesActividad_ActividadConvenioId_EstudianteConv~",
                table: "ParticipacionesActividad",
                columns: new[] { "ActividadConvenioId", "EstudianteConvenioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParticipacionesActividad_EstudianteConvenioId",
                table: "ParticipacionesActividad",
                column: "EstudianteConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsablesConvenio_ConvenioId",
                table: "ResponsablesConvenio",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsablesConvenio_ParteConvenioId",
                table: "ResponsablesConvenio",
                column: "ParteConvenioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmbitosConvenio");

            migrationBuilder.DropTable(
                name: "CierresConvenio");

            migrationBuilder.DropTable(
                name: "ClausulasConvenio");

            migrationBuilder.DropTable(
                name: "ConveniosRelacionados");

            migrationBuilder.DropTable(
                name: "EvaluacionesConvenio");

            migrationBuilder.DropTable(
                name: "FirmantesConvenio");

            migrationBuilder.DropTable(
                name: "ModificacionesConvenio");

            migrationBuilder.DropTable(
                name: "ObligacionesConvenio");

            migrationBuilder.DropTable(
                name: "ParticipacionesActividad");

            migrationBuilder.DropTable(
                name: "ResponsablesConvenio");

            migrationBuilder.DropTable(
                name: "ActividadesConvenio");

            migrationBuilder.DropTable(
                name: "EstudiantesConvenio");

            migrationBuilder.DropTable(
                name: "PartesConvenio");

            migrationBuilder.DropColumn(
                name: "CondicionesRenovacion",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "ConfidencialidadIndefinida",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "DomicilioContractual",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "FechaSuscripcion",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "FechaUltimaFirma",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "FuenteFinanciamiento",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "MecanismoSolucionControversias",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "Moneda",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "Presupuesto",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "TieneErogacion",
                table: "Convenios");
        }
    }
}
