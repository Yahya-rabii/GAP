﻿// <auto-generated />
using System;
using GAP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GAP.Migrations
{
    [DbContext(typeof(GAPContext))]
    partial class GAPContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.6.23329.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GAP.Models.Bill", b =>
                {
                    b.Property<int>("BillID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BillID"));

                    b.Property<int>("FinanceDepartmentManagerId")
                        .HasColumnType("int");

                    b.Property<int?>("PurchaseQuoteID")
                        .HasColumnType("int");

                    b.Property<bool>("Validity")
                        .HasColumnType("bit");

                    b.HasKey("BillID");

                    b.HasIndex("FinanceDepartmentManagerId");

                    b.ToTable("Bill");
                });

            modelBuilder.Entity("GAP.Models.Notification", b =>
                {
                    b.Property<int>("NotificationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationID"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("NotificationTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationID");

                    b.ToTable("Notification");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Notification");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("GAP.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<string>("Desc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ItemsNumber")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PurchaseQuoteID")
                        .HasColumnType("int");

                    b.Property<int?>("SaleOfferID")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<float>("Totalprice")
                        .HasColumnType("real");

                    b.Property<float>("Unitprice")
                        .HasColumnType("real");

                    b.HasKey("ProductID");

                    b.HasIndex("PurchaseQuoteID");

                    b.HasIndex("SaleOfferID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("GAP.Models.PurchaseQuote", b =>
                {
                    b.Property<int>("PurchaseQuoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PurchaseQuoteID"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PurchasingDepartmentManagerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReceptionDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SaleOfferID")
                        .HasColumnType("int");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("int");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<int?>("typeCntProducts")
                        .HasColumnType("int");

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PurchaseRequestID"));

                    b.Property<double>("Budget")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<int?>("PurchasingDepartmentManagerUserID")
                        .HasColumnType("int");

                    b.HasKey("PurchaseRequestID");

                    b.HasIndex("PurchasingDepartmentManagerUserID");

                    b.ToTable("PurchaseRequest");
                });

            modelBuilder.Entity("GAP.Models.QualityTestReport", b =>
                {
                    b.Property<int>("QualityTestReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QualityTestReportID"));

                    b.Property<bool>("CntItemsValidity")
                        .HasColumnType("bit");

                    b.Property<bool>("OperationValidity")
                        .HasColumnType("bit");

                    b.Property<int>("PurchaseQuoteId")
                        .HasColumnType("int");

                    b.Property<int?>("QualityTestingDepartmentManagerId")
                        .HasColumnType("int");

                    b.Property<bool>("StateValidity")
                        .HasColumnType("bit");

                    b.HasKey("QualityTestReportID");

                    b.HasIndex("QualityTestingDepartmentManagerId");

                    b.ToTable("QualityTestReport");
                });

            modelBuilder.Entity("GAP.Models.ReceptionReport", b =>
                {
                    b.Property<int>("ReceptionReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReceptionReportID"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PurchaseQuoteId")
                        .HasColumnType("int");

                    b.Property<int>("PurchasingReceptionistId")
                        .HasColumnType("int");

                    b.HasKey("ReceptionReportID");

                    b.HasIndex("PurchasingReceptionistId");

                    b.ToTable("ReceptionReport");
                });

            modelBuilder.Entity("GAP.Models.SaleOffer", b =>
                {
                    b.Property<int>("SaleOfferID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SaleOfferID"));

                    b.Property<int>("PurchaseRequestId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<double>("TotalProfit")
                        .HasColumnType("float");

                    b.Property<double>("UnitProfit")
                        .HasColumnType("float");

                    b.Property<bool>("Validity")
                        .HasColumnType("bit");

                    b.HasKey("SaleOfferID");

                    b.HasIndex("PurchaseRequestId");

                    b.HasIndex("SupplierId");

                    b.ToTable("SaleOffer");
                });

            modelBuilder.Entity("GAP.Models.Sanction", b =>
                {
                    b.Property<int>("SanctionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SanctionID"));

                    b.Property<int>("PurchaseQuoteID")
                        .HasColumnType("int");

                    b.Property<string>("SanctionDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SanctionTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SupplierId")
                        .HasColumnType("int");

                    b.HasKey("SanctionID");

                    b.ToTable("Sanction");
                });

            modelBuilder.Entity("GAP.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SupplierID"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("PostalCode")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("TransactionNumber")
                        .HasColumnType("int");

                    b.HasKey("SupplierID");

                    b.ToTable("Supplier");
                });

            modelBuilder.Entity("GAP.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("nvarchar(34)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("HasCustomProfilePicture")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfilePictureFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.ToTable("User");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("GAP.Models.NotificationAdmin", b =>
                {
                    b.HasBaseType("GAP.Models.Notification");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("NotificationAdmin");
                });

            modelBuilder.Entity("GAP.Models.NotificationInfo", b =>
                {
                    b.HasBaseType("GAP.Models.Notification");

                    b.Property<int?>("PurchaseQuoteID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("NotificationInfo");
                });

            modelBuilder.Entity("GAP.Models.NotificationSupplier", b =>
                {
                    b.HasBaseType("GAP.Models.Notification");

                    b.Property<int?>("SaleOfferID")
                        .HasColumnType("int");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("int");

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
                    b.HasOne("GAP.Models.PurchaseQuote", null)
                        .WithMany("Products")
                        .HasForeignKey("PurchaseQuoteID");

                    b.HasOne("GAP.Models.SaleOffer", null)
                        .WithMany("Products")
                        .HasForeignKey("SaleOfferID");
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

            modelBuilder.Entity("GAP.Models.PurchaseQuote", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GAP.Models.SaleOffer", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("GAP.Models.FinanceDepartmentManager", b =>
                {
                    b.Navigation("HistoriqueBills");
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
