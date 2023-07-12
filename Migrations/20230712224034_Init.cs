using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GAP.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fournisseur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<int>(type: "int", nullable: false),
                    NombreTransaction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tutulaire = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "OffreVente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    PrixTTL = table.Column<double>(type: "float", nullable: false),
                    Validite = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffreVente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffreVente_Fournisseur_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "Fournisseur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemandeAchat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    RespServiceAchatUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandeAchat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandeAchat_User_RespServiceAchatUserID",
                        column: x => x.RespServiceAchatUserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "RapportReception",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceptServiceAchatId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportReception", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RapportReception_User_ReceptServiceAchatId",
                        column: x => x.ReceptServiceAchatId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RapportTestQualite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ValiditeEtat = table.Column<bool>(type: "bit", nullable: false),
                    ValiditeNbrPiece = table.Column<bool>(type: "bit", nullable: false),
                    ValiditeFonctionnement = table.Column<bool>(type: "bit", nullable: false),
                    RespServiceQualiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapportTestQualite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RapportTestQualite_User_RespServiceQualiteId",
                        column: x => x.RespServiceQualiteId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrixUnitaire = table.Column<int>(type: "int", nullable: false),
                    PrixTotal = table.Column<int>(type: "int", nullable: false),
                    OffreVenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produit_OffreVente_OffreVenteId",
                        column: x => x.OffreVenteId,
                        principalTable: "OffreVente",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Devis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RespServiceAchatId = table.Column<int>(type: "int", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProduitId = table.Column<int>(type: "int", nullable: false),
                    PrixTTL = table.Column<double>(type: "float", nullable: false),
                    DateReception = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    NombrePiece = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devis_Fournisseur_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "Fournisseur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devis_Produit_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "Produit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devis_User_RespServiceAchatId",
                        column: x => x.RespServiceAchatId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProduitId = table.Column<int>(type: "int", nullable: false),
                    Prix = table.Column<double>(type: "float", nullable: false),
                    BeneficiaireId = table.Column<int>(type: "int", nullable: false),
                    Validite = table.Column<bool>(type: "bit", nullable: false),
                    RespServiceFinanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facture_Fournisseur_BeneficiaireId",
                        column: x => x.BeneficiaireId,
                        principalTable: "Fournisseur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facture_Produit_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "Produit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facture_User_RespServiceFinanceId",
                        column: x => x.RespServiceFinanceId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandeAchat_RespServiceAchatUserID",
                table: "DemandeAchat",
                column: "RespServiceAchatUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Devis_FournisseurId",
                table: "Devis",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_Devis_ProduitId",
                table: "Devis",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_Devis_RespServiceAchatId",
                table: "Devis",
                column: "RespServiceAchatId");

            migrationBuilder.CreateIndex(
                name: "IX_Facture_BeneficiaireId",
                table: "Facture",
                column: "BeneficiaireId");

            migrationBuilder.CreateIndex(
                name: "IX_Facture_ProduitId",
                table: "Facture",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_Facture_RespServiceFinanceId",
                table: "Facture",
                column: "RespServiceFinanceId");

            migrationBuilder.CreateIndex(
                name: "IX_OffreVente_FournisseurId",
                table: "OffreVente",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_Produit_OffreVenteId",
                table: "Produit",
                column: "OffreVenteId");

            migrationBuilder.CreateIndex(
                name: "IX_RapportReception_ReceptServiceAchatId",
                table: "RapportReception",
                column: "ReceptServiceAchatId");

            migrationBuilder.CreateIndex(
                name: "IX_RapportTestQualite_RespServiceQualiteId",
                table: "RapportTestQualite",
                column: "RespServiceQualiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandeAchat");

            migrationBuilder.DropTable(
                name: "Devis");

            migrationBuilder.DropTable(
                name: "Facture");

            migrationBuilder.DropTable(
                name: "RapportReception");

            migrationBuilder.DropTable(
                name: "RapportTestQualite");

            migrationBuilder.DropTable(
                name: "Produit");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "OffreVente");

            migrationBuilder.DropTable(
                name: "Fournisseur");
        }
    }
}
