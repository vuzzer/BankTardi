using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BanqueTardi.Migrations
{
    /// <inheritdoc />
    public partial class cleaning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssuranceClient",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    ClientID = table.Column<string>(type: "TEXT", nullable: false),
                    NomClient = table.Column<string>(type: "TEXT", nullable: false),
                    PrenomClient = table.Column<string>(type: "TEXT", nullable: false),
                    DateDeNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CodePartenaire = table.Column<string>(type: "TEXT", nullable: false),
                    CodeRabais = table.Column<int>(type: "INTEGER", nullable: false),
                    Solde = table.Column<double>(type: "REAL", nullable: false),
                    Sexe = table.Column<int>(type: "INTEGER", nullable: false),
                    EstFumeur = table.Column<bool>(type: "INTEGER", nullable: false),
                    EstDiabetique = table.Column<bool>(type: "INTEGER", nullable: false),
                    EstHypertendu = table.Column<bool>(type: "INTEGER", nullable: false),
                    PratiqueActivitePhysique = table.Column<bool>(type: "INTEGER", nullable: false),
                    Statut = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssuranceClient", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientID = table.Column<string>(type: "TEXT", nullable: false),
                    NomClient = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    PrenomClient = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Adresse = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    CodePostal = table.Column<string>(type: "TEXT", nullable: false),
                    NbDecouverts = table.Column<int>(type: "INTEGER", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    NomParent = table.Column<string>(type: "TEXT", nullable: true),
                    TelephoneParent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientID);
                });

            migrationBuilder.CreateTable(
                name: "Compte",
                columns: table => new
                {
                    CompteID = table.Column<string>(type: "TEXT", nullable: false),
                    NumeroCompte = table.Column<string>(type: "TEXT", nullable: false),
                    ClientID = table.Column<string>(type: "TEXT", nullable: false),
                    Solde = table.Column<double>(type: "REAL", nullable: false),
                    Identifiant = table.Column<int>(type: "INTEGER", nullable: false),
                    Libelle = table.Column<string>(type: "TEXT", nullable: false),
                    TauxInteret = table.Column<int>(type: "INTEGER", nullable: false),
                    TauxInteretDecouvert = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeCompte = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compte", x => x.CompteID);
                    table.ForeignKey(
                        name: "FK_Compte_Client_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Client",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operation",
                columns: table => new
                {
                    OperationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompteID = table.Column<string>(type: "TEXT", nullable: false),
                    Libelle = table.Column<string>(type: "TEXT", nullable: false),
                    Montant = table.Column<double>(type: "REAL", nullable: false),
                    DateTransaction = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TypeOperation = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operation", x => x.OperationID);
                    table.ForeignKey(
                        name: "FK_Operation_Compte_CompteID",
                        column: x => x.CompteID,
                        principalTable: "Compte",
                        principalColumn: "CompteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compte_ClientID",
                table: "Compte",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Operation_CompteID",
                table: "Operation",
                column: "CompteID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssuranceClient");

            migrationBuilder.DropTable(
                name: "Operation");

            migrationBuilder.DropTable(
                name: "Compte");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
