using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GAP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NotificationTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 34, nullable: false),
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: true),
                    PurchaseQuoteID = table.Column<int>(type: "INTEGER", nullable: true),
                    ServiceQuoteID = table.Column<int>(type: "INTEGER", nullable: true),
                    NotificationSupplier_SupplierID = table.Column<int>(type: "INTEGER", nullable: true),
                    SaleOfferID = table.Column<int>(type: "INTEGER", nullable: true),
                    ServiceOfferID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                });

            migrationBuilder.CreateTable(
                name: "Reclamation",
                columns: table => new
                {
                    ReclamationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReclamationTitle = table.Column<string>(type: "TEXT", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BugPicture = table.Column<byte[]>(type: "BLOB", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reclamation", x => x.ReclamationID);
                });

            migrationBuilder.CreateTable(
                name: "ReclamationReply",
                columns: table => new
                {
                    ReclamationReplyID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReclamationID = table.Column<int>(type: "INTEGER", nullable: true),
                    Answer = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReclamationReply", x => x.ReclamationReplyID);
                });

            migrationBuilder.CreateTable(
                name: "ReclamationsHistory",
                columns: table => new
                {
                    ReclamationsHistoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReclamationsID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReclamationsHistory", x => x.ReclamationsHistoryID);
                });

            migrationBuilder.CreateTable(
                name: "Sanction",
                columns: table => new
                {
                    SanctionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SanctionTitle = table.Column<string>(type: "TEXT", nullable: false),
                    SanctionDescription = table.Column<string>(type: "TEXT", nullable: false),
                    PurchaseQuoteID = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sanction", x => x.SanctionID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequest",
                columns: table => new
                {
                    ServiceRequestID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    ServiceRequestPicture = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequest", x => x.ServiceRequestID);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StockID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfilePicture = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ProfilePictureFileName = table.Column<string>(type: "TEXT", nullable: true),
                    HasCustomProfilePicture = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 34, nullable: false),
                    ProjectManagerID = table.Column<int>(type: "INTEGER", nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Adresse = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PostalCode = table.Column<int>(type: "INTEGER", nullable: true),
                    PhoneNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    TransactionNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Bill",
                columns: table => new
                {
                    BillID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Validity = table.Column<bool>(type: "INTEGER", nullable: false),
                    FinanceDepartmentManagerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    PurchaseQuoteID = table.Column<int>(type: "INTEGER", nullable: true),
                    ServiceQuoteID = table.Column<int>(type: "INTEGER", nullable: true)
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
                    ProjectID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Budget = table.Column<double>(type: "REAL", nullable: false),
                    ProjectManagerUserID = table.Column<int>(type: "INTEGER", nullable: true)
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
                    PurchaseRequestID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Budget = table.Column<double>(type: "REAL", nullable: false),
                    IsValid = table.Column<bool>(type: "INTEGER", nullable: false),
                    PurchasingDepartmentManagerUserID = table.Column<int>(type: "INTEGER", nullable: true)
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
                    QualityTestReportID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StateValidity = table.Column<bool>(type: "INTEGER", nullable: false),
                    CntItemsValidity = table.Column<bool>(type: "INTEGER", nullable: false),
                    OperationValidity = table.Column<bool>(type: "INTEGER", nullable: false),
                    QualityTestingDepartmentManagerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PurchaseQuoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceQuoteId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    ReceptionReportID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PurchasingReceptionistId = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseQuoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceQuoteId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "ServiceOffer",
                columns: table => new
                {
                    ServiceOfferID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Validity = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOffer", x => x.ServiceOfferID);
                    table.ForeignKey(
                        name: "FK_ServiceOffer_ServiceRequest_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequest",
                        principalColumn: "ServiceRequestID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOffer_User_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleOffer",
                columns: table => new
                {
                    SaleOfferID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitProfit = table.Column<double>(type: "REAL", nullable: false),
                    TotalProfit = table.Column<double>(type: "REAL", nullable: false),
                    Validity = table.Column<bool>(type: "INTEGER", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseRequestId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "ServiceQuote",
                columns: table => new
                {
                    ServiceQuoteID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: true),
                    PurchasingDepartmentManagerId = table.Column<int>(type: "INTEGER", nullable: true),
                    ServiceOfferID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceQuote", x => x.ServiceQuoteID);
                    table.ForeignKey(
                        name: "FK_ServiceQuote_ServiceOffer_ServiceOfferID",
                        column: x => x.ServiceOfferID,
                        principalTable: "ServiceOffer",
                        principalColumn: "ServiceOfferID");
                    table.ForeignKey(
                        name: "FK_ServiceQuote_User_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseQuote",
                columns: table => new
                {
                    PurchaseQuoteID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceptionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<double>(type: "REAL", nullable: true),
                    typeCntProducts = table.Column<int>(type: "INTEGER", nullable: true),
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: true),
                    PurchasingDepartmentManagerId = table.Column<int>(type: "INTEGER", nullable: true),
                    SaleOfferID = table.Column<int>(type: "INTEGER", nullable: true)
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
                    ProductID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Unitprice = table.Column<float>(type: "REAL", nullable: false),
                    Totalprice = table.Column<float>(type: "REAL", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ItemsNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    Desc = table.Column<string>(type: "TEXT", nullable: true),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductPicture = table.Column<byte[]>(type: "BLOB", nullable: true),
                    ProjectID = table.Column<int>(type: "INTEGER", nullable: true),
                    PurchaseQuoteID = table.Column<int>(type: "INTEGER", nullable: true),
                    SaleOfferID = table.Column<int>(type: "INTEGER", nullable: true),
                    StockID = table.Column<int>(type: "INTEGER", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOffer_ServiceRequestId",
                table: "ServiceOffer",
                column: "ServiceRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOffer_SupplierId",
                table: "ServiceOffer",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceQuote_ServiceOfferID",
                table: "ServiceQuote",
                column: "ServiceOfferID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceQuote_SupplierID",
                table: "ServiceQuote",
                column: "SupplierID");
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
                name: "ReclamationsHistory");

            migrationBuilder.DropTable(
                name: "Sanction");

            migrationBuilder.DropTable(
                name: "ServiceQuote");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "PurchaseQuote");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "ServiceOffer");

            migrationBuilder.DropTable(
                name: "SaleOffer");

            migrationBuilder.DropTable(
                name: "ServiceRequest");

            migrationBuilder.DropTable(
                name: "PurchaseRequest");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
