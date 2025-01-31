using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouTubeFullApplication.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Docenti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CognomeNome = table.Column<string>(type: "TEXT", maxLength: 66, nullable: false, computedColumnSql: "upper(Cognome || ' ' || Nome)"),
                    CodiceFiscale = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    DataNascita = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Cognome = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    CognomeNome = table.Column<string>(type: "TEXT", maxLength: 66, nullable: false, computedColumnSql: "upper(Cognome || ' ' || Nome)"),
                    CodiceFiscale = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    DataNascita = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Abbinamenti",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Classe_Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Docente_Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Materia_Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abbinamenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abbinamenti_Classi_Classe_Id",
                        column: x => x.Classe_Id,
                        principalTable: "Classi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Abbinamenti_Docenti_Docente_Id",
                        column: x => x.Docente_Id,
                        principalTable: "Docenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Abbinamenti_Materie_Materia_Id",
                        column: x => x.Materia_Id,
                        principalTable: "Materie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Frequenze",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Studente_Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Classe_Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnnoScolastico = table.Column<int>(type: "INTEGER", nullable: false),
                    Esito = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequenze", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Frequenze_Classi_Classe_Id",
                        column: x => x.Classe_Id,
                        principalTable: "Classi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Frequenze_Studenti_Studente_Id",
                        column: x => x.Studente_Id,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abbinamenti_Classe_Id",
                table: "Abbinamenti",
                column: "Classe_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Abbinamenti_Docente_Id",
                table: "Abbinamenti",
                column: "Docente_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Abbinamenti_Materia_Id",
                table: "Abbinamenti",
                column: "Materia_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Classi_Nome",
                table: "Classi",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Docenti_CodiceFiscale",
                table: "Docenti",
                column: "CodiceFiscale",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Docenti_CognomeNome",
                table: "Docenti",
                column: "CognomeNome");

            migrationBuilder.CreateIndex(
                name: "IX_Frequenze_Classe_Id",
                table: "Frequenze",
                column: "Classe_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Frequenze_Studente_Id",
                table: "Frequenze",
                column: "Studente_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Materie_Nome",
                table: "Materie",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_CodiceFiscale",
                table: "Studenti",
                column: "CodiceFiscale",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_CognomeNome",
                table: "Studenti",
                column: "CognomeNome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abbinamenti");

            migrationBuilder.DropTable(
                name: "Frequenze");

            migrationBuilder.DropTable(
                name: "Docenti");

            migrationBuilder.DropTable(
                name: "Materie");

            migrationBuilder.DropTable(
                name: "Classi");

            migrationBuilder.DropTable(
                name: "Studenti");
        }
    }
}
