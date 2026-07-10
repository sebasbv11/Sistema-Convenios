using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaConvenios.Migrations
{
    /// <inheritdoc />
    public partial class AjustesConveniosReportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactoGestionCargo",
                table: "Entidades",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoGestionEmail",
                table: "Entidades",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoGestionNombre",
                table: "Entidades",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoGestionTelefono",
                table: "Entidades",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provincia",
                table: "Entidades",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelefonoRepresentante",
                table: "Entidades",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AreaPromotoraId",
                table: "Convenios",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ContactoGestionEmail",
                table: "Convenios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoGestionNombre",
                table: "Convenios",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoGestionTelefono",
                table: "Convenios",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConvenioPadreId",
                table: "Convenios",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AreasPromotoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreasPromotoras", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AreasPromotoras",
                columns: new[] { "Id", "Activo", "Codigo", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "VIN-001", "Vinculación con la sociedad" },
                    { 2, true, "PRA-001", "Prácticas preprofesionales" },
                    { 3, true, "INV-001", "Investigación" },
                    { 4, true, "ACA-001", "Académica" }
                });

            migrationBuilder.Sql("""
                UPDATE "Convenios"
                SET "Estado" = 'Vigente'
                WHERE "Estado" IN ('Borrador', 'En revisión', 'Aprobado');
                """);

            migrationBuilder.Sql("""
                UPDATE "Entidades"
                SET "Provincia" = CASE
                    WHEN NULLIF(TRIM("Ciudad"), '') ILIKE 'manta' THEN 'Manabí'
                    ELSE 'No registrada'
                END
                WHERE "Provincia" IS NULL OR TRIM("Provincia") = '';
                """);

            migrationBuilder.Sql("""
                UPDATE "Entidades"
                SET "TelefonoRepresentante" = COALESCE(NULLIF(TRIM("Telefono"), ''), 'No registrado')
                WHERE "TelefonoRepresentante" IS NULL OR TRIM("TelefonoRepresentante") = '';
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Convenios_AreaPromotoraId",
                table: "Convenios",
                column: "AreaPromotoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Convenios_ConvenioPadreId",
                table: "Convenios",
                column: "ConvenioPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_AreasPromotoras_Codigo",
                table: "AreasPromotoras",
                column: "Codigo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Convenios_AreasPromotoras_AreaPromotoraId",
                table: "Convenios",
                column: "AreaPromotoraId",
                principalTable: "AreasPromotoras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Convenios_Convenios_ConvenioPadreId",
                table: "Convenios",
                column: "ConvenioPadreId",
                principalTable: "Convenios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Convenios_AreasPromotoras_AreaPromotoraId",
                table: "Convenios");

            migrationBuilder.DropForeignKey(
                name: "FK_Convenios_Convenios_ConvenioPadreId",
                table: "Convenios");

            migrationBuilder.DropTable(
                name: "AreasPromotoras");

            migrationBuilder.DropIndex(
                name: "IX_Convenios_AreaPromotoraId",
                table: "Convenios");

            migrationBuilder.DropIndex(
                name: "IX_Convenios_ConvenioPadreId",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "ContactoGestionCargo",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "ContactoGestionEmail",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "ContactoGestionNombre",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "ContactoGestionTelefono",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "Provincia",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "TelefonoRepresentante",
                table: "Entidades");

            migrationBuilder.DropColumn(
                name: "AreaPromotoraId",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "ContactoGestionEmail",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "ContactoGestionNombre",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "ContactoGestionTelefono",
                table: "Convenios");

            migrationBuilder.DropColumn(
                name: "ConvenioPadreId",
                table: "Convenios");
        }
    }
}
