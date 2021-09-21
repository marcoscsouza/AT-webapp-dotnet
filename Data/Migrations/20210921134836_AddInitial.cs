using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Data.Migrations
{
    public partial class AddInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Musicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UltimoNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantosInstrumentos = table.Column<int>(type: "int", nullable: false),
                    PossuiBanda = table.Column<bool>(type: "bit", nullable: false),
                    ImageUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visualizacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musicos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Musicos");
        }
    }
}
