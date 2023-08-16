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
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    PurchaseQuoteID = table.Column<int>(type: "int", nullable: true),
                    NotificationSupplier_SupplierID = table.Column<int>(type: "int", nullable: true),
                    SaleOfferID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                });

            migrationBuilder.CreateTable(
                name: "Reclamation",
                columns: table => new
                {
                    ReclamationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReclamationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BugPicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reclamation", x => x.ReclamationID);
                });

            migrationBuilder.CreateTable(
                name: "ReclamationReply",
                columns: table => new
                {
                    ReclamationReplyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReclamationID = table.Column<int>(type: "int", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReclamationReply", x => x.ReclamationReplyID);
                });

            migrationBuilder.CreateTable(
                name: "Sanction",
                columns: table => new
                {
                    SanctionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanctionTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SanctionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseQuoteID = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sanction", x => x.SanctionID);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockID);
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
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfilePictureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasCustomProfilePicture = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    ProjectManagerID = table.Column<int>(type: "int", nullable: true),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PostalCode = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: true),
                    TransactionNumber = table.Column<int>(type: "int", nullable: true),
                    IsValid = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    BillID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Validity = table.Column<bool>(type: "bit", nullable: false),
                    FinanceDepartmentManagerId = table.Column<int>(type: "int", nullable: false),
                    PurchaseQuoteID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bill", x => x.BillID);
                    table.ForeignKey(
                        name: "FK_Bill_User_FinanceDepartmentManagerId",
                        column: x => x.FinanceDepartmentManagerId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    ProjectManagerUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Project_User_ProjectManagerUserID",
                        column: x => x.ProjectManagerUserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequest",
                columns: table => new
                {
                    PurchaseRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Budget = table.Column<double>(type: "float", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    PurchasingDepartmentManagerUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequest", x => x.PurchaseRequestID);
                    table.ForeignKey(
                        name: "FK_PurchaseRequest_User_PurchasingDepartmentManagerUserID",
                        column: x => x.PurchasingDepartmentManagerUserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "QualityTestReport",
                columns: table => new
                {
                    QualityTestReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateValidity = table.Column<bool>(type: "bit", nullable: false),
                    CntItemsValidity = table.Column<bool>(type: "bit", nullable: false),
                    OperationValidity = table.Column<bool>(type: "bit", nullable: false),
                    QualityTestingDepartmentManagerId = table.Column<int>(type: "int", nullable: true),
                    PurchaseQuoteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityTestReport", x => x.QualityTestReportID);
                    table.ForeignKey(
                        name: "FK_QualityTestReport_User_QualityTestingDepartmentManagerId",
                        column: x => x.QualityTestingDepartmentManagerId,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "ReceptionReport",
                columns: table => new
                {
                    ReceptionReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasingReceptionistId = table.Column<int>(type: "int", nullable: false),
                    PurchaseQuoteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceptionReport", x => x.ReceptionReportID);
                    table.ForeignKey(
                        name: "FK_ReceptionReport_User_PurchasingReceptionistId",
                        column: x => x.PurchasingReceptionistId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleOffer",
                columns: table => new
                {
                    SaleOfferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitProfit = table.Column<double>(type: "float", nullable: false),
                    TotalProfit = table.Column<double>(type: "float", nullable: false),
                    Validity = table.Column<bool>(type: "bit", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleOffer", x => x.SaleOfferID);
                    table.ForeignKey(
                        name: "FK_SaleOffer_PurchaseRequest_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalTable: "PurchaseRequest",
                        principalColumn: "PurchaseRequestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleOffer_User_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseQuote",
                columns: table => new
                {
                    PurchaseQuoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: true),
                    typeCntProducts = table.Column<int>(type: "int", nullable: true),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    PurchasingDepartmentManagerId = table.Column<int>(type: "int", nullable: true),
                    SaleOfferID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseQuote", x => x.PurchaseQuoteID);
                    table.ForeignKey(
                        name: "FK_PurchaseQuote_SaleOffer_SaleOfferID",
                        column: x => x.SaleOfferID,
                        principalTable: "SaleOffer",
                        principalColumn: "SaleOfferID");
                    table.ForeignKey(
                        name: "FK_PurchaseQuote_User_PurchasingDepartmentManagerId",
                        column: x => x.PurchasingDepartmentManagerId,
                        principalTable: "User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_PurchaseQuote_User_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unitprice = table.Column<float>(type: "real", nullable: false),
                    Totalprice = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemsNumber = table.Column<int>(type: "int", nullable: true),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProjectID = table.Column<int>(type: "int", nullable: true),
                    PurchaseQuoteID = table.Column<int>(type: "int", nullable: true),
                    SaleOfferID = table.Column<int>(type: "int", nullable: true),
                    StockID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ProjectID");
                    table.ForeignKey(
                        name: "FK_Product_PurchaseQuote_PurchaseQuoteID",
                        column: x => x.PurchaseQuoteID,
                        principalTable: "PurchaseQuote",
                        principalColumn: "PurchaseQuoteID");
                    table.ForeignKey(
                        name: "FK_Product_SaleOffer_SaleOfferID",
                        column: x => x.SaleOfferID,
                        principalTable: "SaleOffer",
                        principalColumn: "SaleOfferID");
                    table.ForeignKey(
                        name: "FK_Product_Stock_StockID",
                        column: x => x.StockID,
                        principalTable: "Stock",
                        principalColumn: "StockID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_FinanceDepartmentManagerId",
                table: "Bill",
                column: "FinanceDepartmentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProjectID",
                table: "Product",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_PurchaseQuoteID",
                table: "Product",
                column: "PurchaseQuoteID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SaleOfferID",
                table: "Product",
                column: "SaleOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_StockID",
                table: "Product",
                column: "StockID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectManagerUserID",
                table: "Project",
                column: "ProjectManagerUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseQuote_PurchasingDepartmentManagerId",
                table: "PurchaseQuote",
                column: "PurchasingDepartmentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseQuote_SaleOfferID",
                table: "PurchaseQuote",
                column: "SaleOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseQuote_SupplierID",
                table: "PurchaseQuote",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequest_PurchasingDepartmentManagerUserID",
                table: "PurchaseRequest",
                column: "PurchasingDepartmentManagerUserID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityTestReport_QualityTestingDepartmentManagerId",
                table: "QualityTestReport",
                column: "QualityTestingDepartmentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceptionReport_PurchasingReceptionistId",
                table: "ReceptionReport",
                column: "PurchasingReceptionistId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOffer_PurchaseRequestId",
                table: "SaleOffer",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleOffer_SupplierId",
                table: "SaleOffer",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bill");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "QualityTestReport");

            migrationBuilder.DropTable(
                name: "ReceptionReport");

            migrationBuilder.DropTable(
                name: "Reclamation");

            migrationBuilder.DropTable(
                name: "ReclamationReply");

            migrationBuilder.DropTable(
                name: "Sanction");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "PurchaseQuote");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "SaleOffer");

            migrationBuilder.DropTable(
                name: "PurchaseRequest");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
