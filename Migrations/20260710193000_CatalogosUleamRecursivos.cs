using Microsoft.EntityFrameworkCore.Migrations;
using SistemaConvenios.Data;

#nullable disable

namespace SistemaConvenios.Migrations;

/// <inheritdoc />
public partial class CatalogosUleamRecursivos : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "AreaPadreId",
            table: "AreasPromotoras",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Tipo",
            table: "AreasPromotoras",
            type: "character varying(50)",
            maxLength: 50,
            nullable: true);

        foreach (var area in CatalogoSeedData.AreasPromotoras.Where(x => Id(x) >= 1000))
        {
            migrationBuilder.InsertData(
                table: "AreasPromotoras",
                columns: new[] { "Id", "Codigo", "Nombre", "Tipo", "AreaPadreId", "Activo" },
                values: new[] { Id(area), Value(area, "Codigo"), Value(area, "Nombre"), Value(area, "Tipo"), Value(area, "AreaPadreId"), Value(area, "Activo") });
        }

        foreach (var facultad in CatalogoSeedData.Facultades.Where(x => Id(x) >= 1000))
        {
            migrationBuilder.InsertData(
                table: "Facultades",
                columns: new[] { "Id", "Nombre", "Siglas", "Activo" },
                values: new[] { Id(facultad), Value(facultad, "Nombre"), Value(facultad, "Siglas"), Value(facultad, "Activo") });
        }

        foreach (var carrera in CatalogoSeedData.Carreras.Where(x => Id(x) >= 100000))
        {
            migrationBuilder.InsertData(
                table: "Carreras",
                columns: new[] { "Id", "Nombre", "Siglas", "FacultadId", "Activo" },
                values: new[] { Id(carrera), Value(carrera, "Nombre"), Value(carrera, "Siglas"), Value(carrera, "FacultadId"), Value(carrera, "Activo") });
        }

        foreach (var area in CatalogoSeedData.AreasPromotoras.Where(x => Id(x) is >= 1 and <= 4))
        {
            migrationBuilder.UpdateData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: Id(area),
                columns: new[] { "AreaPadreId", "Tipo" },
                values: new[] { Value(area, "AreaPadreId"), Value(area, "Tipo") });
        }

        migrationBuilder.UpdateData(
            table: "Facultades",
            keyColumn: "Id",
            keyValue: 1,
            column: "Activo",
            value: false);

        foreach (var carrera in CatalogoSeedData.Carreras.Where(x => Id(x) is >= 1 and <= 3))
        {
            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: Id(carrera),
                column: "Activo",
                value: false);
        }

        migrationBuilder.CreateIndex(
            name: "IX_AreasPromotoras_AreaPadreId",
            table: "AreasPromotoras",
            column: "AreaPadreId");

        migrationBuilder.AddForeignKey(
            name: "FK_AreasPromotoras_AreasPromotoras_AreaPadreId",
            table: "AreasPromotoras",
            column: "AreaPadreId",
            principalTable: "AreasPromotoras",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AreasPromotoras_AreasPromotoras_AreaPadreId",
            table: "AreasPromotoras");

        migrationBuilder.DropIndex(
            name: "IX_AreasPromotoras_AreaPadreId",
            table: "AreasPromotoras");

        foreach (var carrera in CatalogoSeedData.Carreras.Where(x => Id(x) >= 100000))
        {
            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: Id(carrera));
        }

        foreach (var facultad in CatalogoSeedData.Facultades.Where(x => Id(x) >= 1000))
        {
            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: Id(facultad));
        }

        foreach (var area in CatalogoSeedData.AreasPromotoras.Where(x => Id(x) >= 1000))
        {
            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: Id(area));
        }

        migrationBuilder.UpdateData(
            table: "Facultades",
            keyColumn: "Id",
            keyValue: 1,
            column: "Activo",
            value: true);

        foreach (var carrera in CatalogoSeedData.Carreras.Where(x => Id(x) is >= 1 and <= 3))
        {
            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: Id(carrera),
                column: "Activo",
                value: true);
        }

        for (var id = 1; id <= 4; id++)
        {
            migrationBuilder.UpdateData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: id,
                columns: new[] { "AreaPadreId", "Tipo" },
                values: new object[] { null!, null! });
        }

        migrationBuilder.DropColumn(
            name: "AreaPadreId",
            table: "AreasPromotoras");

        migrationBuilder.DropColumn(
            name: "Tipo",
            table: "AreasPromotoras");
    }

    private static int Id(object row) => (int)Value(row, "Id")!;

    private static object Value(object row, string name) =>
        row.GetType().GetProperty(name)?.GetValue(row);
}
