﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shinsekai_API.Models;

namespace Shinsekai_API.Migrations
{
    [DbContext(typeof(ShinsekaiApiContext))]
    partial class ShinsekaiApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Shinsekai_API.Models.AnimeArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("AnimeId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("AnimeId");

                    b.HasIndex("ArticleId");

                    b.ToTable("AnimesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AnimeItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Animes");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("BrandId")
                        .HasColumnType("nvarchar(36)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal>("DiscountPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Height")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("OriginalFlag")
                        .HasColumnType("bit");

                    b.Property<string>("OriginalSerial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AuthParamItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("AuthParams");
                });

            modelBuilder.Entity("Shinsekai_API.Models.BrandItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Shinsekai_API.Models.CarouselItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RedirectPath")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Carousels");
                });

            modelBuilder.Entity("Shinsekai_API.Models.DeliveryItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<int>("EstimatedDays")
                        .HasColumnType("int");

                    b.Property<string>("LocationId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Parcel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId")
                        .IsUnique();

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Shinsekai_API.Models.FavoriteItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ImageItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LineArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("LineId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("LineId");

                    b.ToTable("LinesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LineItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LocationItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Shinsekai_API.Models.MaterialArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("MaterialId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("MaterialId");

                    b.ToTable("MaterialsArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.MaterialItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Shinsekai_API.Models.OriginalItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId")
                        .IsUnique();

                    b.ToTable("Originals");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PointItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PromotionItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RedirectPath")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("PurchaseId")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("PurchasesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<int>("CashPoints")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Shinsekai_API.Models.QuestionItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Shinsekai_API.Models.RequestItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PurchaseId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Shinsekai_API.Models.SaleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<DateTime>("SoldDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ShoppingCartArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("ShoppingCartArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.UserItem", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Address")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("Admin")
                        .HasColumnType("bit");

                    b.Property<string>("AuthParamsId")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Phone")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.HasKey("Id");

                    b.HasIndex("AuthParamsId")
                        .IsUnique()
                        .HasFilter("[AuthParamsId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AnimeArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.AnimeItem", "Anime")
                        .WithMany("AnimesArticles")
                        .HasForeignKey("AnimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("AnimesArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Anime");

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.BrandItem", "Brand")
                        .WithMany("Articles")
                        .HasForeignKey("BrandId");

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("Shinsekai_API.Models.DeliveryItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.LocationItem", "Location")
                        .WithOne("Delivery")
                        .HasForeignKey("Shinsekai_API.Models.DeliveryItem", "LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Shinsekai_API.Models.FavoriteItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("Favorites")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shinsekai_API.Models.UserItem", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ImageItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("Images")
                        .HasForeignKey("ArticleId");

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LineArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("LinesArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shinsekai_API.Models.LineItem", "Line")
                        .WithMany("LinesArticles")
                        .HasForeignKey("LineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Line");
                });

            modelBuilder.Entity("Shinsekai_API.Models.MaterialArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("MaterialsArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shinsekai_API.Models.MaterialItem", "Material")
                        .WithMany("Materials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("Shinsekai_API.Models.OriginalItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithOne("Original")
                        .HasForeignKey("Shinsekai_API.Models.OriginalItem", "ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PointItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.UserItem", "User")
                        .WithMany("Points")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("PurchasesArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shinsekai_API.Models.PurchaseItem", "Purchase")
                        .WithMany("PurchasesArticles")
                        .HasForeignKey("PurchaseId");

                    b.Navigation("Article");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.UserItem", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shinsekai_API.Models.RequestItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.PurchaseItem", "Purchase")
                        .WithMany("Requests")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("Shinsekai_API.Models.SaleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("Sales")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ShoppingCartArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("ShoppingCartsArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shinsekai_API.Models.UserItem", "User")
                        .WithMany("SoppingCartArticles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shinsekai_API.Models.UserItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.AuthParamItem", "AuthParams")
                        .WithOne("User")
                        .HasForeignKey("Shinsekai_API.Models.UserItem", "AuthParamsId");

                    b.Navigation("AuthParams");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AnimeItem", b =>
                {
                    b.Navigation("AnimesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ArticleItem", b =>
                {
                    b.Navigation("AnimesArticles");

                    b.Navigation("Favorites");

                    b.Navigation("Images");

                    b.Navigation("LinesArticles");

                    b.Navigation("MaterialsArticles");

                    b.Navigation("Original");

                    b.Navigation("PurchasesArticles");

                    b.Navigation("Sales");

                    b.Navigation("ShoppingCartsArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AuthParamItem", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Shinsekai_API.Models.BrandItem", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LineItem", b =>
                {
                    b.Navigation("LinesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LocationItem", b =>
                {
                    b.Navigation("Delivery");
                });

            modelBuilder.Entity("Shinsekai_API.Models.MaterialItem", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseItem", b =>
                {
                    b.Navigation("PurchasesArticles");

                    b.Navigation("Requests");
                });

            modelBuilder.Entity("Shinsekai_API.Models.UserItem", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Points");

                    b.Navigation("Purchases");

                    b.Navigation("SoppingCartArticles");
                });
#pragma warning restore 612, 618
        }
    }
}
