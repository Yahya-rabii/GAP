﻿// <auto-generated />
using System;
using GAP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GAP.Migrations
{
    [DbContext(typeof(GAPContext))]
    [Migration("20250421235554_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-preview.6.23329.4");

            modelBuilder.Entity("GAP.Models.Bill", b =>
                {
                    b.Property<int>("BillID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.Property<int>("FinanceDepartmentManagerId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Validity")
                        .HasColumnType("INTEGER");

                    b.HasKey("BillID");

                    b.HasIndex("FinanceDepartmentManagerId");

                    b.ToTable("Bill");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Bill");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("GAP.Models.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("TEXT");

                    b.Property<string>("NotificationTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("NotificationID");

                    b.ToTable("Notification");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Notification");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("GAP.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Desc")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ItemsNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("ProductPicture")
                        .HasColumnType("BLOB");

                    b.Property<int?>("ProjectID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PurchaseQuoteID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SaleOfferID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("StockID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Totalprice")
                        .HasColumnType("REAL");

                    b.Property<float>("Unitprice")
                        .HasColumnType("REAL");

                    b.HasKey("ProductID");

                    b.HasIndex("ProjectID");

                    b.HasIndex("PurchaseQuoteID");

                    b.HasIndex("SaleOfferID");

                    b.HasIndex("StockID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("GAP.Models.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Budget")
                        .HasColumnType("REAL");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProjectManagerUserID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("ProjectID");

                    b.HasIndex("ProjectManagerUserID");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("GAP.Models.PurchaseQuote", b =>
                {
                    b.Property<int>("PurchaseQuoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PurchasingDepartmentManagerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ReceptionDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SaleOfferID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("REAL");

                    b.Property<int?>("typeCntProducts")
                        .HasColumnType("INTEGER");

                    b.HasKey("PurchaseQuoteID");

                    b.HasIndex("PurchasingDepartmentManagerId");

                    b.HasIndex("SaleOfferID");

                    b.HasIndex("SupplierID");

                    b.ToTable("PurchaseQuote");
                });

            modelBuilder.Entity("GAP.Models.PurchaseRequest", b =>
                {
                    b.Property<int>("PurchaseRequestID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Budget")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PurchasingDepartmentManagerUserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("PurchaseRequestID");

                    b.HasIndex("PurchasingDepartmentManagerUserID");

                    b.ToTable("PurchaseRequest");
                });

            modelBuilder.Entity("GAP.Models.QualityTestReport", b =>
                {
                    b.Property<int>("QualityTestReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CntItemsValidity")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("OperationValidity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchaseQuoteId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("QualityTestingDepartmentManagerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServiceQuoteId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("StateValidity")
                        .HasColumnType("INTEGER");

                    b.HasKey("QualityTestReportID");

                    b.HasIndex("QualityTestingDepartmentManagerId");

                    b.ToTable("QualityTestReport");
                });

            modelBuilder.Entity("GAP.Models.ReceptionReport", b =>
                {
                    b.Property<int>("ReceptionReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("PurchaseQuoteId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchasingReceptionistId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServiceQuoteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReceptionReportID");

                    b.HasIndex("PurchasingReceptionistId");

                    b.ToTable("ReceptionReport");
                });

            modelBuilder.Entity("GAP.Models.Reclamation", b =>
                {
                    b.Property<int>("ReclamationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("BugPicture")
                        .HasColumnType("BLOB");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReclamationTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReclamationID");

                    b.ToTable("Reclamation");
                });

            modelBuilder.Entity("GAP.Models.ReclamationReply", b =>
                {
                    b.Property<int>("ReclamationReplyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ReclamationID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReclamationReplyID");

                    b.ToTable("ReclamationReply");
                });

            modelBuilder.Entity("GAP.Models.ReclamationsHistory", b =>
                {
                    b.Property<int>("ReclamationsHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReclamationsID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReclamationsHistoryID");

                    b.ToTable("ReclamationsHistory");
                });

            modelBuilder.Entity("GAP.Models.SaleOffer", b =>
                {
                    b.Property<int>("SaleOfferID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchaseRequestId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalProfit")
                        .HasColumnType("REAL");

                    b.Property<double>("UnitProfit")
                        .HasColumnType("REAL");

                    b.Property<bool>("Validity")
                        .HasColumnType("INTEGER");

                    b.HasKey("SaleOfferID");

                    b.HasIndex("PurchaseRequestId");

                    b.HasIndex("SupplierId");

                    b.ToTable("SaleOffer");
                });

            modelBuilder.Entity("GAP.Models.Sanction", b =>
                {
                    b.Property<int>("SanctionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PurchaseQuoteID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SanctionDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SanctionTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SanctionID");

                    b.ToTable("Sanction");
                });

            modelBuilder.Entity("GAP.Models.ServiceOffer", b =>
                {
                    b.Property<int>("ServiceOfferID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("ServiceRequestId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Validity")
                        .HasColumnType("INTEGER");

                    b.HasKey("ServiceOfferID");

                    b.HasIndex("ServiceRequestId");

                    b.HasIndex("SupplierId");

                    b.ToTable("ServiceOffer");
                });

            modelBuilder.Entity("GAP.Models.ServiceQuote", b =>
                {
                    b.Property<int>("ServiceQuoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Price")
                        .HasColumnType("REAL");

                    b.Property<int?>("PurchasingDepartmentManagerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ServiceOfferID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ServiceQuoteID");

                    b.HasIndex("ServiceOfferID");

                    b.HasIndex("SupplierID");

                    b.ToTable("ServiceQuote");
                });

            modelBuilder.Entity("GAP.Models.ServiceRequest", b =>
                {
                    b.Property<int>("ServiceRequestID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ServiceRequestPicture")
                        .HasColumnType("BLOB");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("ServiceRequestID");

                    b.ToTable("ServiceRequest");
                });

            modelBuilder.Entity("GAP.Models.Stock", b =>
                {
                    b.Property<int>("StockID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("StockID");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("GAP.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasCustomProfilePicture")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("BLOB");

                    b.Property<string>("ProfilePictureFileName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserID");

                    b.ToTable("User");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("GAP.Models.BillPurchase", b =>
                {
                    b.HasBaseType("GAP.Models.Bill");

                    b.Property<int>("PurchaseQuoteID")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("BillPurchase");
                });

            modelBuilder.Entity("GAP.Models.BillService", b =>
                {
                    b.HasBaseType("GAP.Models.Bill");

                    b.Property<int>("ServiceQuoteID")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("BillService");
                });

            modelBuilder.Entity("GAP.Models.NotificationAdmin", b =>
                {
                    b.HasBaseType("GAP.Models.Notification");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("NotificationAdmin");
                });

            modelBuilder.Entity("GAP.Models.NotificationInfo", b =>
                {
                    b.HasBaseType("GAP.Models.Notification");

                    b.Property<int?>("PurchaseQuoteID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ServiceQuoteID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("NotificationInfo");
                });

            modelBuilder.Entity("GAP.Models.NotificationReclamation", b =>
                {
                    b.HasBaseType("GAP.Models.Notification");

                    b.HasDiscriminator().HasValue("NotificationReclamation");
                });

            modelBuilder.Entity("GAP.Models.NotificationSupplier", b =>
                {
                    b.HasBaseType("GAP.Models.Notification");

                    b.Property<int?>("SaleOfferID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ServiceOfferID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("INTEGER");

                    b.ToTable("Notification", t =>
                        {
                            t.Property("SupplierID")
                                .HasColumnName("NotificationSupplier_SupplierID");
                        });

                    b.HasDiscriminator().HasValue("NotificationSupplier");
                });

            modelBuilder.Entity("GAP.Models.FinanceDepartmentManager", b =>
                {
                    b.HasBaseType("GAP.Models.User");

                    b.HasDiscriminator().HasValue("FinanceDepartmentManager");
                });

            modelBuilder.Entity("GAP.Models.ProjectManager", b =>
                {
                    b.HasBaseType("GAP.Models.User");

                    b.Property<int>("ProjectManagerID")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("ProjectManager");
                });

            modelBuilder.Entity("GAP.Models.PurchasingDepartmentManager", b =>
                {
                    b.HasBaseType("GAP.Models.User");

                    b.HasDiscriminator().HasValue("PurchasingDepartmentManager");
                });

            modelBuilder.Entity("GAP.Models.PurchasingReceptionist", b =>
                {
                    b.HasBaseType("GAP.Models.User");

                    b.HasDiscriminator().HasValue("PurchasingReceptionist");
                });

            modelBuilder.Entity("GAP.Models.QualityTestingDepartmentManager", b =>
                {
                    b.HasBaseType("GAP.Models.User");

                    b.HasDiscriminator().HasValue("QualityTestingDepartmentManager");
                });

            modelBuilder.Entity("GAP.Models.Supplier", b =>
                {
                    b.HasBaseType("GAP.Models.User");

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PostalCode")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TransactionNumber")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Supplier");
                });

            modelBuilder.Entity("GAP.Models.Bill", b =>
                {
                    b.HasOne("GAP.Models.FinanceDepartmentManager", null)
                        .WithMany("HistoriqueBills")
                        .HasForeignKey("FinanceDepartmentManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GAP.Models.Product", b =>
                {
                    b.HasOne("GAP.Models.Project", null)
                        .WithMany("Products")
                        .HasForeignKey("ProjectID");

                    b.HasOne("GAP.Models.PurchaseQuote", null)
                        .WithMany("Products")
                        .HasForeignKey("PurchaseQuoteID");

                    b.HasOne("GAP.Models.SaleOffer", null)
                        .WithMany("Products")
                        .HasForeignKey("SaleOfferID");

                    b.HasOne("GAP.Models.Stock", null)
                        .WithMany("Products")
                        .HasForeignKey("StockID");
                });

            modelBuilder.Entity("GAP.Models.Project", b =>
                {
                    b.HasOne("GAP.Models.ProjectManager", null)
                        .WithMany("Projects")
                        .HasForeignKey("ProjectManagerUserID");
                });

            modelBuilder.Entity("GAP.Models.PurchaseQuote", b =>
                {
                    b.HasOne("GAP.Models.PurchasingDepartmentManager", null)
                        .WithMany("PurchaseQuotes")
                        .HasForeignKey("PurchasingDepartmentManagerId");

                    b.HasOne("GAP.Models.SaleOffer", "SaleOffer")
                        .WithMany()
                        .HasForeignKey("SaleOfferID");

                    b.HasOne("GAP.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierID");

                    b.Navigation("SaleOffer");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GAP.Models.PurchaseRequest", b =>
                {
                    b.HasOne("GAP.Models.PurchasingDepartmentManager", null)
                        .WithMany("DemandesAchats")
                        .HasForeignKey("PurchasingDepartmentManagerUserID");
                });

            modelBuilder.Entity("GAP.Models.QualityTestReport", b =>
                {
                    b.HasOne("GAP.Models.QualityTestingDepartmentManager", null)
                        .WithMany("HistoriqueRapportQualite")
                        .HasForeignKey("QualityTestingDepartmentManagerId");
                });

            modelBuilder.Entity("GAP.Models.ReceptionReport", b =>
                {
                    b.HasOne("GAP.Models.PurchasingReceptionist", null)
                        .WithMany("HistoriqueRapportsReceptions")
                        .HasForeignKey("PurchasingReceptionistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GAP.Models.SaleOffer", b =>
                {
                    b.HasOne("GAP.Models.PurchaseRequest", "PurchaseRequest")
                        .WithMany()
                        .HasForeignKey("PurchaseRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GAP.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PurchaseRequest");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GAP.Models.ServiceOffer", b =>
                {
                    b.HasOne("GAP.Models.ServiceRequest", "ServiceRequest")
                        .WithMany()
                        .HasForeignKey("ServiceRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GAP.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceRequest");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GAP.Models.ServiceQuote", b =>
                {
                    b.HasOne("GAP.Models.ServiceOffer", "ServiceOffer")
                        .WithMany()
                        .HasForeignKey("ServiceOfferID");

                    b.HasOne("GAP.Models.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierID");

                    b.Navigation("ServiceOffer");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GAP.Models.Project", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GAP.Models.PurchaseQuote", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GAP.Models.SaleOffer", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GAP.Models.Stock", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GAP.Models.FinanceDepartmentManager", b =>
                {
                    b.Navigation("HistoriqueBills");
                });

            modelBuilder.Entity("GAP.Models.ProjectManager", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("GAP.Models.PurchasingDepartmentManager", b =>
                {
                    b.Navigation("DemandesAchats");

                    b.Navigation("PurchaseQuotes");
                });

            modelBuilder.Entity("GAP.Models.PurchasingReceptionist", b =>
                {
                    b.Navigation("HistoriqueRapportsReceptions");
                });

            modelBuilder.Entity("GAP.Models.QualityTestingDepartmentManager", b =>
                {
                    b.Navigation("HistoriqueRapportQualite");
                });
#pragma warning restore 612, 618
        }
    }
}
