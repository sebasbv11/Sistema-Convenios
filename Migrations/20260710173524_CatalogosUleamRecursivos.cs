using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SistemaConvenios.Migrations
{
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

            migrationBuilder.UpdateData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AreaPadreId", "Tipo" },
                values: new object[] { 1015, "Área interna" });

            migrationBuilder.UpdateData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AreaPadreId", "Tipo" },
                values: new object[] { 1015, "Área interna" });

            migrationBuilder.UpdateData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AreaPadreId", "Tipo" },
                values: new object[] { 1015, "Área interna" });

            migrationBuilder.UpdateData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AreaPadreId", "Tipo" },
                values: new object[] { 1015, "Área interna" });

            migrationBuilder.InsertData(
                table: "AreasPromotoras",
                columns: new[] { "Id", "Activo", "AreaPadreId", "Codigo", "Nombre", "Tipo" },
                values: new object[] { 1000, false, null, "ULEAM", "Universidad Laica Eloy Alfaro de Manabí", "Institución" });

            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 1,
                column: "Activo",
                value: false);

            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 2,
                column: "Activo",
                value: false);

            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 3,
                column: "Activo",
                value: false);

            migrationBuilder.UpdateData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1,
                column: "Activo",
                value: false);

            migrationBuilder.InsertData(
                table: "Facultades",
                columns: new[] { "Id", "Activo", "Nombre", "Siglas" },
                values: new object[,]
                {
                    { 1001, true, "AREAS DE LA SALUD", "AS" },
                    { 1002, true, "ARQUITECTURA", "A" },
                    { 1003, true, "ARTES, HUMANIDADES Y PATRIMONIO", "AHP" },
                    { 1004, true, "BAHÍA DE CARÁQUEZ", "BC" },
                    { 1005, true, "CAMPUS FLAVIO ALFARO", "CFA" },
                    { 1006, true, "CAMPUS JUNÍN", "CJ" },
                    { 1007, true, "CAMPUS PICHINCHA", "CP" },
                    { 1008, true, "CHONE", "C" },
                    { 1009, true, "CIENCIAS ADMINISTRATIVAS", "CA" },
                    { 1010, true, "CIENCIAS ADMINISTRATIVAS, CONTABLES Y COMERCIO", "CACC" },
                    { 1011, true, "CIENCIAS AGROPECUARIAS", "CA" },
                    { 1012, true, "CIENCIAS DE LA COMUNICACIÓN", "CC" },
                    { 1013, true, "CIENCIAS DE LA EDUCACIÓN", "CE" },
                    { 1014, true, "CIENCIAS DE LA SALUD", "CS" },
                    { 1015, true, "CIENCIAS DE LA VIDA Y TECNOLOGÍAS", "CVT" },
                    { 1016, true, "CIENCIAS DEL MAR", "CM" },
                    { 1017, true, "CIENCIAS ECONÓMICAS", "CE" },
                    { 1018, true, "CIENCIAS INFORMÁTICAS", "CI" },
                    { 1019, true, "CIENCIAS MÉDICAS", "CM" },
                    { 1020, true, "CIENCIAS SOCIALES, DERECHO Y BIENESTAR", "CSDB" },
                    { 1021, true, "COMERCIO EXTERIOR Y NEGOCIOS INTERNACIONALES", "CENI" },
                    { 1022, true, "CONTABILIDAD Y AUDITORÍA", "CA" },
                    { 1023, true, "DERECHO", "D" },
                    { 1024, true, "EDUCACIÓN FÍSICA DEPORTES Y RECREACIÓN", "EFDR" },
                    { 1025, true, "EDUCACIÓN Y TURISMO", "ET" },
                    { 1026, true, "EL CARMEN", "C" },
                    { 1027, true, "GESTIÓN ORGANIZACIONAL", "GO" },
                    { 1028, true, "HOTELERÍA Y TURISMO", "HT" },
                    { 1029, true, "INGENIERÍA", "I" },
                    { 1030, true, "INGENIERÍA INDUSTRIAL", "II" },
                    { 1031, true, "INGENIERÍA, INDUSTRIA, Y ARQUITECTURA", "IIA" },
                    { 1032, true, "INSTITUTO DE IDIOMAS", "II" },
                    { 1033, true, "MECÁNICA NAVAL", "MN" },
                    { 1034, true, "ODONTOLOGÍA", "O" },
                    { 1035, true, "PEDERNALES", "P" },
                    { 1036, true, "PSICOLOGÍA", "P" },
                    { 1037, true, "SEDE SANTO DOMINGO DE LOS TSÁCHILAS", "SSDT" },
                    { 1038, true, "TRABAJO SOCIAL", "TS" },
                    { 1039, true, "UNIDAD ACADÉMICA DE FORMACIÓN TÉCNICA Y TECNOLÓGICA, EDUCACIÓN VIRTUAL Y OTRAS MODALIDADES DE ESTUDIO", "UAFTTEVOME" }
                });

            migrationBuilder.InsertData(
                table: "AreasPromotoras",
                columns: new[] { "Id", "Activo", "AreaPadreId", "Codigo", "Nombre", "Tipo" },
                values: new object[,]
                {
                    { 1001, true, 1000, "ULEAM-001", "AREAS DE LA SALUD", "Unidad académica" },
                    { 1002, true, 1000, "ULEAM-002", "ARQUITECTURA", "Unidad académica" },
                    { 1003, true, 1000, "ULEAM-003", "ARTES, HUMANIDADES Y PATRIMONIO", "Unidad académica" },
                    { 1004, true, 1000, "ULEAM-004", "BAHÍA DE CARÁQUEZ", "Unidad académica" },
                    { 1005, true, 1000, "ULEAM-005", "CAMPUS FLAVIO ALFARO", "Unidad académica" },
                    { 1006, true, 1000, "ULEAM-006", "CAMPUS JUNÍN", "Unidad académica" },
                    { 1007, true, 1000, "ULEAM-007", "CAMPUS PICHINCHA", "Unidad académica" },
                    { 1008, true, 1000, "ULEAM-008", "CHONE", "Unidad académica" },
                    { 1009, true, 1000, "ULEAM-009", "CIENCIAS ADMINISTRATIVAS", "Unidad académica" },
                    { 1010, true, 1000, "ULEAM-010", "CIENCIAS ADMINISTRATIVAS, CONTABLES Y COMERCIO", "Unidad académica" },
                    { 1011, true, 1000, "ULEAM-011", "CIENCIAS AGROPECUARIAS", "Unidad académica" },
                    { 1012, true, 1000, "ULEAM-012", "CIENCIAS DE LA COMUNICACIÓN", "Unidad académica" },
                    { 1013, true, 1000, "ULEAM-013", "CIENCIAS DE LA EDUCACIÓN", "Unidad académica" },
                    { 1014, true, 1000, "ULEAM-014", "CIENCIAS DE LA SALUD", "Unidad académica" },
                    { 1015, true, 1000, "ULEAM-015", "CIENCIAS DE LA VIDA Y TECNOLOGÍAS", "Unidad académica" },
                    { 1016, true, 1000, "ULEAM-016", "CIENCIAS DEL MAR", "Unidad académica" },
                    { 1017, true, 1000, "ULEAM-017", "CIENCIAS ECONÓMICAS", "Unidad académica" },
                    { 1018, true, 1000, "ULEAM-018", "CIENCIAS INFORMÁTICAS", "Unidad académica" },
                    { 1019, true, 1000, "ULEAM-019", "CIENCIAS MÉDICAS", "Unidad académica" },
                    { 1020, true, 1000, "ULEAM-020", "CIENCIAS SOCIALES, DERECHO Y BIENESTAR", "Unidad académica" },
                    { 1021, true, 1000, "ULEAM-021", "COMERCIO EXTERIOR Y NEGOCIOS INTERNACIONALES", "Unidad académica" },
                    { 1022, true, 1000, "ULEAM-022", "CONTABILIDAD Y AUDITORÍA", "Unidad académica" },
                    { 1023, true, 1000, "ULEAM-023", "DERECHO", "Unidad académica" },
                    { 1024, true, 1000, "ULEAM-024", "EDUCACIÓN FÍSICA DEPORTES Y RECREACIÓN", "Unidad académica" },
                    { 1025, true, 1000, "ULEAM-025", "EDUCACIÓN Y TURISMO", "Unidad académica" },
                    { 1026, true, 1000, "ULEAM-026", "EL CARMEN", "Unidad académica" },
                    { 1027, true, 1000, "ULEAM-027", "GESTIÓN ORGANIZACIONAL", "Unidad académica" },
                    { 1028, true, 1000, "ULEAM-028", "HOTELERÍA Y TURISMO", "Unidad académica" },
                    { 1029, true, 1000, "ULEAM-029", "INGENIERÍA", "Unidad académica" },
                    { 1030, true, 1000, "ULEAM-030", "INGENIERÍA INDUSTRIAL", "Unidad académica" },
                    { 1031, true, 1000, "ULEAM-031", "INGENIERÍA, INDUSTRIA, Y ARQUITECTURA", "Unidad académica" },
                    { 1032, true, 1000, "ULEAM-032", "INSTITUTO DE IDIOMAS", "Unidad académica" },
                    { 1033, true, 1000, "ULEAM-033", "MECÁNICA NAVAL", "Unidad académica" },
                    { 1034, true, 1000, "ULEAM-034", "ODONTOLOGÍA", "Unidad académica" },
                    { 1035, true, 1000, "ULEAM-035", "PEDERNALES", "Unidad académica" },
                    { 1036, true, 1000, "ULEAM-036", "PSICOLOGÍA", "Unidad académica" },
                    { 1037, true, 1000, "ULEAM-037", "SEDE SANTO DOMINGO DE LOS TSÁCHILAS", "Unidad académica" },
                    { 1038, true, 1000, "ULEAM-038", "TRABAJO SOCIAL", "Unidad académica" },
                    { 1039, true, 1000, "ULEAM-039", "UNIDAD ACADÉMICA DE FORMACIÓN TÉCNICA Y TECNOLÓGICA, EDUCACIÓN VIRTUAL Y OTRAS MODALIDADES DE ESTUDIO", "Unidad académica" }
                });

            migrationBuilder.InsertData(
                table: "Carreras",
                columns: new[] { "Id", "Activo", "FacultadId", "Nombre", "Siglas" },
                values: new object[,]
                {
                    { 100001, true, 1001, "NUTRICION Y DIETETICA", "ND" },
                    { 100002, true, 1001, "TERAPIA OCUPACIONAL", "TO" },
                    { 100003, true, 1001, "TERAPIA DEL LENGUAJE", "TL" },
                    { 100004, true, 1001, "RADIOLOGIA E IMAGENOLOGIA", "RI" },
                    { 100005, true, 1001, "FISIOTERAPIA", "F" },
                    { 100006, true, 1001, "LABORATORIO CLINICO", "LC" },
                    { 100007, true, 1002, "ARQUITECTURA", "A" },
                    { 100008, true, 1003, "ARTES PLÁSTICAS", "AP" },
                    { 100009, true, 1003, "ARTES ESCÉNICAS", "AE" },
                    { 100010, true, 1003, "DISEÑO TEXTIL E INDUMENTARIA", "DTI" },
                    { 100011, true, 1003, "ARQUEOLOGIA", "A" },
                    { 100012, true, 1003, "SOCIOLOGIA", "S" },
                    { 100013, true, 1004, "ARQUEOLOGIA", "A" },
                    { 100014, true, 1004, "GESTIÓN DEL TALENTO HUMANO", "GTH" },
                    { 100015, true, 1004, "HOSPITALIDAD Y HOTELERIA", "HH" },
                    { 100016, true, 1004, "CONTABILIDAD Y AUDITORIA", "CA" },
                    { 100017, true, 1004, "DERECHO", "D" },
                    { 100018, true, 1004, "TURISMO", "T" },
                    { 100019, true, 1004, "CIENCIAS DE LA COMUNICACION MENCION PUBLICIDAD Y MERCADOTECNIA", "CCMPM" },
                    { 100020, true, 1004, "FISIOTERAPIA", "F" },
                    { 100021, true, 1004, "EDUCACION INICIAL", "EI" },
                    { 100022, true, 1004, "ADMINISTRACIÓN DE EMPRESAS", "AE" },
                    { 100023, true, 1004, "EDUCACION BASICA", "EB" },
                    { 100024, true, 1004, "ENFERMERÍA", "E" },
                    { 100025, true, 1004, "GASTRONOMIA", "G" },
                    { 100026, true, 1004, "MERCADOTECNIA", "M" },
                    { 100027, true, 1004, "CONSTRUCCION SISMORRESISTENTE", "CS" },
                    { 100028, true, 1004, "AGRONEGOCIOS", "A" },
                    { 100029, true, 1004, "TURISMO SOSTENIBLE", "TS" },
                    { 100030, true, 1005, "EDUCACION INICIAL", "EI" },
                    { 100031, true, 1005, "SOFTWARE", "S" },
                    { 100032, true, 1006, "EDUCACION BASICA", "EB" },
                    { 100033, true, 1007, "AGRONEGOCIOS", "A" },
                    { 100034, true, 1007, "EDUCACION BASICA", "EB" },
                    { 100035, true, 1008, "ELECTRICIDAD", "E" },
                    { 100036, true, 1008, "ARTES PLÁSTICAS", "AP" },
                    { 100037, true, 1008, "PEDAGOGÍA DE LAS CIENCIAS EXPERIMENTALES", "PCE" },
                    { 100038, true, 1008, "ADMINISTRACIÓN DE EMPRESAS", "AE" },
                    { 100039, true, 1008, "ODONTOLOGIA", "O" },
                    { 100040, true, 1008, "TECNOLOGÍAS DE LA INFORMACIÓN", "TI" },
                    { 100041, true, 1008, "DERECHO", "D" },
                    { 100042, true, 1008, "ARQUITECTURA", "A" },
                    { 100043, true, 1008, "INGENIERÍA CIVIL", "IC" },
                    { 100044, true, 1008, "EDUCACION BASICA BILINGÜE", "EBB" },
                    { 100045, true, 1008, "MEDICINA", "M" },
                    { 100046, true, 1008, "PSICOLOGÍA EDUCATIVA", "PE" },
                    { 100047, true, 1008, "ALIMENTOS", "A" },
                    { 100048, true, 1008, "SOFTWARE", "S" },
                    { 100049, true, 1008, "NUTRICION Y DIETETICA", "ND" },
                    { 100050, true, 1008, "EDUCACION BASICA", "EB" },
                    { 100051, true, 1008, "EDUCACION INICIAL", "EI" },
                    { 100052, true, 1008, "PEDAGOGÍA DE LOS IDIOMAS NACIONALES Y EXTRANJEROS", "PINE" },
                    { 100053, true, 1008, "AGRONEGOCIOS", "A" },
                    { 100054, true, 1008, "INGENIERIA ELECTRICA", "IE" },
                    { 100055, true, 1008, "INGENIERIA CIVIL", "IC" },
                    { 100056, true, 1008, "ENFERMERÍA", "E" },
                    { 100057, true, 1008, "CONTABILIDAD Y AUDITORIA", "CA" },
                    { 100058, true, 1008, "FISIOTERAPIA", "F" },
                    { 100059, true, 1008, "ENTRENAMIENTO DEPORTIVO", "ED" },
                    { 100060, true, 1008, "AGROPECUARIA", "A" },
                    { 100061, true, 1008, "INGENIERIA AGROPECUARIA", "IA" },
                    { 100062, true, 1008, "INGENIERIA EN SISTEMAS", "IS" },
                    { 100063, true, 1008, "EDUCACIÓN INICIAL BILINGÜE", "EIB" },
                    { 100064, true, 1009, "INGENIERIA EN MARKETING", "IM" },
                    { 100065, true, 1009, "ADMINISTRACION DE EMPRESAS", "AE" },
                    { 100066, true, 1010, "GESTIÓN DE LA INFORMACIÓN GERENCIAL", "GIG" },
                    { 100067, true, 1010, "FINANZAS", "F" },
                    { 100068, true, 1010, "CONTABILIDAD Y AUDITORIA", "CA" },
                    { 100069, true, 1010, "COMERCIO EXTERIOR", "CE" },
                    { 100070, true, 1010, "SERVICIOS GERENCIALES", "SG" },
                    { 100071, true, 1010, "ADMINISTRACIÓN DE EMPRESAS", "AE" },
                    { 100072, true, 1010, "AUDITORÍA Y CONTROL DE GESTIÓN", "ACG" },
                    { 100073, true, 1010, "MERCADOTECNIA", "M" },
                    { 100074, true, 1010, "GESTIÓN DEL TALENTO HUMANO", "GTH" },
                    { 100075, true, 1010, "BIENES RAICES", "BR" },
                    { 100076, true, 1011, "INGENIERIA AGROPECUARIA", "IA" },
                    { 100077, true, 1011, "INGENIERIA AGROINDUSTRIAL", "IA" },
                    { 100078, true, 1012, "CIENCIAS DE LA COMUNICACION MENCION PERIODISMO", "CCMP" },
                    { 100079, true, 1012, "CIENCIAS DE LA COMUNICACION MENCION PUBLICIDAD Y MERCADOTECNIA", "CCMPM" },
                    { 100080, true, 1012, "CIENCIAS DE LA COMUNICACION MENCION COMUNICACION ORGANIZACIONAL Y RELACIONES PUBLICAS", "CCMCORP" },
                    { 100081, true, 1012, "CIENCIAS DE LA COMUNICACION MENCION COMUNICACION ORGANIZACIONAL Y RELACIONES PUBLICA", "CCMCORP" },
                    { 100082, true, 1013, "EDUCACION PARVULARIA", "EP" },
                    { 100083, true, 1013, "EDUCACION BASICA", "EB" },
                    { 100084, true, 1013, "CIENCIAS DE LA EDUCACION MENCION COMPUTACION COMERCIO Y ADMINISTRACION", "CEMCCA" },
                    { 100085, true, 1013, "036-2019", "02" },
                    { 100086, true, 1013, "EDUCACION ESPECIAL", "EE" },
                    { 100087, true, 1013, "CIENCIAS DE LA EDUCACION MENCION PSICOPEDAGOGIA Y TECNICAS DE LA ENSEÑANZA", "CEMPTE" },
                    { 100088, true, 1013, "CIENCIAS DE LA EDUCACION MENCION FISICO MATEMATICAS", "CEMFM" },
                    { 100089, true, 1013, "CIENCIAS DE LA EDUCACION MENCION PEDAGOGIA", "CEMP" },
                    { 100090, true, 1013, "CIENCIAS DE LA EDUCACION MENCION CASTELLANO Y LITERATURA", "CEMCL" },
                    { 100091, true, 1013, "IDIOMAS MENCION INGLES", "IMI" },
                    { 100092, true, 1013, "CIENCIAS DE LA EDUCACION MENCION CULTURA ESTETICA", "CEMCE" },
                    { 100093, true, 1013, "CIENCIAS DE LA EDUCACION MENCION HISTORIA Y GEOGRAFIA", "CEMHG" },
                    { 100094, true, 1013, "EDUCACION PRIMARIA", "EP" },
                    { 100095, true, 1014, "ODONTOLOGIA", "O" },
                    { 100096, true, 1014, "NUTRICION Y DIETETICA", "ND" },
                    { 100097, true, 1014, "ENFERMERÍA", "E" },
                    { 100098, true, 1014, "PSICOLOGIA", "P" },
                    { 100099, true, 1014, "TERAPIA OCUPACIONAL", "TO" },
                    { 100100, true, 1014, "MEDICINA", "M" },
                    { 100101, true, 1014, "LABORATORIO CLINICO", "LC" },
                    { 100102, true, 1014, "FONOAUDIOLOGÍA", "F" },
                    { 100103, true, 1014, "FISIOTERAPIA", "F" },
                    { 100104, true, 1015, "INGENIERÍA AMBIENTAL", "IA" },
                    { 100105, true, 1015, "AGROPECUARIA", "A" },
                    { 100106, true, 1015, "SOFTWARE", "S" },
                    { 100107, true, 1015, "TECNOLOGÍAS DE LA INFORMACIÓN", "TI" },
                    { 100108, true, 1015, "AGROINDUSTRIA", "A" },
                    { 100109, true, 1015, "BIOLOGÍA", "B" },
                    { 100110, true, 1015, "AGRONEGOCIOS", "A" },
                    { 100111, true, 1016, "BIOQUIMICA EN ACTIVIDADES PESQUERAS", "BAP" },
                    { 100112, true, 1017, "ECONOMIA", "E" },
                    { 100113, true, 1018, "INGENIERIA EN SISTEMAS", "IS" },
                    { 100114, true, 1019, "MEDICINA", "M" },
                    { 100115, true, 1020, "COMUNICACIÓN", "C" },
                    { 100116, true, 1020, "ECONOMIA", "E" },
                    { 100117, true, 1020, "DERECHO", "D" },
                    { 100118, true, 1020, "CIENCIAS DE LA COMUNICACION MENCION PERIODISMO", "CCMP" },
                    { 100119, true, 1020, "COMUNICACIÓN PARA TELEVISIÓN, RELACIONES PÚBLICAS Y PROTOCOLO", "CPTRPP" },
                    { 100120, true, 1020, "TRABAJO SOCIAL", "TS" },
                    { 100121, true, 1020, "CIENCIAS POLÍTICAS Y RELACIONES INTERNACIONALES", "CPRI" },
                    { 100122, true, 1020, "GESTIÓN PÚBLICA Y DESARROLLO", "GPD" },
                    { 100123, true, 1020, "CRIMINOLOGÍA Y CIENCIAS FORENSES", "CCF" },
                    { 100124, true, 1021, "COMERCIO EXTERIOR Y NEGOCIOS INTERNACIONALES", "CENI" },
                    { 100125, true, 1022, "CONTABILIDAD Y AUDITORIA", "CA" },
                    { 100126, true, 1023, "DERECHO", "D" },
                    { 100127, true, 1024, "EDUCACION FISICA DEPORTES Y RECREACION", "EFDR" },
                    { 100128, true, 1024, "EDUCACION FISICA DEPORTES Y RECREACION MENCION ENTRENAMIENTO DEPORTIVO", "EFDRMED" },
                    { 100129, true, 1025, "EDUCACION BASICA", "EB" },
                    { 100130, true, 1025, "EDUCACION INICIAL", "EI" },
                    { 100131, true, 1025, "PEDAGOGÍA DE LA ACTIVIDAD FISICA Y DEPORTE", "PAFD" },
                    { 100132, true, 1025, "GASTRONOMIA", "G" },
                    { 100133, true, 1025, "TURISMO SOSTENIBLE", "TS" },
                    { 100134, true, 1025, "ENTRENAMIENTO DEPORTIVO", "ED" },
                    { 100135, true, 1025, "EDUCACION BASICA BILINGÜE", "EBB" },
                    { 100136, true, 1025, "PEDAGOGÍA DE LOS IDIOMAS NACIONALES Y EXTRANJEROS", "PINE" },
                    { 100137, true, 1025, "HOSPITALIDAD Y HOTELERIA", "HH" },
                    { 100138, true, 1025, "PEDAGOGÍA DE LA LENGUA Y LA LITERATURA", "PLL" },
                    { 100139, true, 1025, "PSICOLOGÍA EDUCATIVA", "PE" },
                    { 100140, true, 1025, "GESTION HOTELERA INTERNACIONAL", "GHI" },
                    { 100141, true, 1025, "EDUCACION ESPECIAL", "EE" },
                    { 100142, true, 1025, "EDUCACIÓN INICIAL BILINGÜE", "EIB" },
                    { 100143, true, 1025, "TURISMO", "T" },
                    { 100144, true, 1025, "EDUCACIÓN INCLUSIVA", "EI" },
                    { 100145, true, 1026, "EDUCACION INICIAL", "EI" },
                    { 100146, true, 1026, "ELECTROMECÁNICA", "E" },
                    { 100147, true, 1026, "TECNOLOGÍAS DE LA INFORMACIÓN", "TI" },
                    { 100148, true, 1026, "ALIMENTOS", "A" },
                    { 100149, true, 1026, "AUDITORÍA Y CONTROL DE GESTIÓN", "ACG" },
                    { 100150, true, 1026, "SOFTWARE", "S" },
                    { 100151, true, 1026, "DERECHO", "D" },
                    { 100152, true, 1026, "AGRONEGOCIOS", "A" },
                    { 100153, true, 1026, "ENFERMERÍA", "E" },
                    { 100154, true, 1026, "ADMINISTRACIÓN DE EMPRESAS", "AE" },
                    { 100155, true, 1026, "FINANZAS", "F" },
                    { 100156, true, 1026, "AGROPECUARIA", "A" },
                    { 100157, true, 1026, "INGENIERIA AGROPECUARIA", "IA" },
                    { 100158, true, 1026, "CONTABILIDAD Y AUDITORIA", "CA" },
                    { 100159, true, 1026, "INGENIERIA EN SISTEMAS", "IS" },
                    { 100160, true, 1026, "EDUCACION PRIMARIA", "EP" },
                    { 100161, true, 1026, "EDUCACION BASICA", "EB" },
                    { 100162, true, 1026, "PSICOLOGÍA EDUCATIVA", "PE" },
                    { 100163, true, 1026, "FISIOTERAPIA", "F" },
                    { 100164, true, 1026, "ELECTROMECÁNICA Y ENERGÍAS RENOVABLES", "EER" },
                    { 100165, true, 1027, "SECRETARIADO EJECUTIVO", "SE" },
                    { 100166, true, 1027, "SERVICIOS GERENCIALES", "SG" },
                    { 100167, true, 1028, "TURISMO", "T" },
                    { 100168, true, 1028, "HOTELERIA", "H" },
                    { 100169, true, 1029, "INGENIERIA ELECTRICA", "IE" },
                    { 100170, true, 1029, "INGENIERIA CIVIL", "IC" },
                    { 100171, true, 1029, "TECNOLOGIA EN CONSTRUCCION CIVIL", "TCC" },
                    { 100172, true, 1030, "INGENIERIA INDUSTRIAL", "II" },
                    { 100173, true, 1030, "INGENIERIA EN ALIMENTOS", "IA" },
                    { 100174, true, 1031, "INGENIERIA CIVIL", "IC" },
                    { 100175, true, 1031, "INGENIERÍA MARÍTIMA", "IM" },
                    { 100176, true, 1031, "ARQUITECTURA", "A" },
                    { 100177, true, 1031, "INGENIERÍA INDUSTRIAL", "II" },
                    { 100178, true, 1031, "ALIMENTOS", "A" },
                    { 100179, true, 1031, "ELECTRICIDAD", "E" },
                    { 100180, true, 1031, "CONSTRUCCION SISMORRESISTENTE", "CS" },
                    { 100181, true, 1031, "METALMECÁNICA", "M" },
                    { 100182, true, 1032, "CORSO DI ITALIANO", "CDI" },
                    { 100183, true, 1032, "SEMINARIOS DE INGLES", "SI" },
                    { 100184, true, 1032, "SELF PACED", "SP" },
                    { 100185, true, 1032, "UEES", "U" },
                    { 100186, true, 1032, "ENGLISH PROFICIENCY", "EP" },
                    { 100187, true, 1032, "N/D", "ND" },
                    { 100188, true, 1032, "SUFFISANCE DE FRANCAIS", "SF" },
                    { 100189, true, 1032, "CONVENIO EP", "CE" },
                    { 100190, true, 1033, "INGENIERIA EN MECANICA NAVAL", "IMN" },
                    { 100191, true, 1034, "ODONTOLOGIA", "O" },
                    { 100192, true, 1035, "AGROPECUARIA", "A" },
                    { 100193, true, 1035, "ARQUITECTURA", "A" },
                    { 100194, true, 1035, "TURISMO", "T" },
                    { 100195, true, 1035, "BIOLOGÍA", "B" },
                    { 100196, true, 1035, "INGENIERIA AGROPECUARIA", "IA" },
                    { 100197, true, 1035, "ADMINISTRACIÓN DE EMPRESAS", "AE" },
                    { 100198, true, 1035, "ECONOMIA", "E" },
                    { 100199, true, 1035, "HOTELERIA Y TURISMO", "HT" },
                    { 100200, true, 1035, "ADMINISTRACION DE EMPRESAS", "AE" },
                    { 100201, true, 1035, "GASTRONOMIA", "G" },
                    { 100202, true, 1035, "FISIOTERAPIA", "F" },
                    { 100203, true, 1035, "ENFERMERÍA", "E" },
                    { 100204, true, 1035, "EDUCACION INICIAL", "EI" },
                    { 100205, true, 1035, "DERECHO", "D" },
                    { 100206, true, 1035, "EDUCACION BASICA", "EB" },
                    { 100207, true, 1035, "ELECTROMECÁNICA", "E" },
                    { 100208, true, 1035, "AGRONEGOCIOS", "A" },
                    { 100209, true, 1035, "ELECTROMECÁNICA Y ENERGÍAS RENOVABLES", "EER" },
                    { 100210, true, 1035, "INGENIERIA AGROPECUARIA ACUICOLA", "IAA" },
                    { 100211, true, 1036, "PSICOLOGIA", "P" },
                    { 100212, true, 1037, "EDUCACION BASICA", "EB" },
                    { 100213, true, 1037, "ADMINISTRACIÓN DE EMPRESAS", "AE" },
                    { 100214, true, 1037, "DISEÑO TEXTIL E INDUMENTARIA", "DTI" },
                    { 100215, true, 1037, "TURISMO SOSTENIBLE", "TS" },
                    { 100216, true, 1037, "INGENIERIA CIVIL", "IC" },
                    { 100217, true, 1037, "ARQUITECTURA", "A" },
                    { 100218, true, 1037, "DERECHO", "D" },
                    { 100219, true, 1037, "FISIOTERAPIA", "F" },
                    { 100220, true, 1037, "BIENES RAICES", "BR" },
                    { 100221, true, 1038, "TRABAJO SOCIAL", "TS" },
                    { 100222, true, 1038, "PSICOLOGIA", "P" },
                    { 100223, true, 1039, "TECNOLOGÍA SUPERIOR EN RIEGO Y PRODUCCIÓN AGRÍCOLA", "TSRPA" },
                    { 100224, true, 1039, "ELECTROMECÁNICA", "E" },
                    { 100225, true, 1039, "EXPLOTACIÓN Y MANTENIMIENTO DE EQUIPOS BIOMÉDICOS", "EMEB" },
                    { 100226, true, 1039, "ELECTROMECÁNICA Y ENERGÍAS RENOVABLES", "EER" },
                    { 100227, true, 1039, "GASTRONOMIA", "G" },
                    { 100228, true, 1039, "Seminario Curriculares", "SC" },
                    { 100229, true, 1039, "EDUCACIÓN VIRTUAL", "EV" },
                    { 100230, true, 1039, "CONSTRUCCION SISMORRESISTENTE", "CS" }
                });

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

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1018);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1019);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1020);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1021);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1022);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1023);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1024);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1025);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1026);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1027);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1028);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1029);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1030);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1031);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1032);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1033);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1034);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1035);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1036);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1037);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1038);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1039);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100001);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100002);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100003);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100004);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100005);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100006);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100007);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100008);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100009);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100010);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100011);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100012);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100013);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100014);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100015);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100016);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100017);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100018);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100019);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100020);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100021);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100022);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100023);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100024);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100025);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100026);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100027);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100028);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100029);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100030);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100031);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100032);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100033);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100034);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100035);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100036);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100037);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100038);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100039);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100040);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100041);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100042);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100043);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100044);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100045);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100046);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100047);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100048);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100049);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100050);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100051);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100052);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100053);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100054);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100055);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100056);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100057);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100058);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100059);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100060);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100061);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100062);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100063);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100064);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100065);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100066);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100067);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100068);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100069);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100070);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100071);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100072);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100073);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100074);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100075);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100076);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100077);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100078);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100079);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100080);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100081);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100082);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100083);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100084);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100085);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100086);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100087);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100088);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100089);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100090);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100091);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100092);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100093);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100094);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100095);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100096);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100097);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100098);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100099);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100100);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100101);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100102);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100103);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100104);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100105);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100106);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100107);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100108);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100109);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100110);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100111);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100112);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100113);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100114);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100115);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100116);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100117);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100118);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100119);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100120);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100121);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100122);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100123);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100124);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100125);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100126);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100127);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100128);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100129);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100130);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100131);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100132);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100133);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100134);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100135);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100136);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100137);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100138);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100139);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100140);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100141);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100142);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100143);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100144);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100145);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100146);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100147);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100148);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100149);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100150);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100151);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100152);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100153);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100154);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100155);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100156);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100157);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100158);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100159);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100160);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100161);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100162);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100163);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100164);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100165);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100166);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100167);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100168);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100169);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100170);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100171);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100172);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100173);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100174);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100175);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100176);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100177);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100178);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100179);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100180);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100181);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100182);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100183);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100184);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100185);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100186);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100187);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100188);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100189);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100190);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100191);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100192);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100193);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100194);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100195);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100196);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100197);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100198);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100199);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100200);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100201);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100202);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100203);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100204);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100205);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100206);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100207);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100208);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100209);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100210);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100211);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100212);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100213);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100214);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100215);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100216);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100217);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100218);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100219);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100220);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100221);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100222);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100223);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100224);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100225);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100226);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100227);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100228);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100229);

            migrationBuilder.DeleteData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 100230);

            migrationBuilder.DeleteData(
                table: "AreasPromotoras",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1018);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1019);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1020);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1021);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1022);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1023);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1024);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1025);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1026);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1027);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1028);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1029);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1030);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1031);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1032);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1033);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1034);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1035);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1036);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1037);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1038);

            migrationBuilder.DeleteData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1039);

            migrationBuilder.DropColumn(
                name: "AreaPadreId",
                table: "AreasPromotoras");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "AreasPromotoras");

            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 1,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 2,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Carreras",
                keyColumn: "Id",
                keyValue: 3,
                column: "Activo",
                value: true);

            migrationBuilder.UpdateData(
                table: "Facultades",
                keyColumn: "Id",
                keyValue: 1,
                column: "Activo",
                value: true);
        }
    }
}
