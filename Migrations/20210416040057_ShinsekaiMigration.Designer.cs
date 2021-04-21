﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shinsekai_API.Models;

namespace Shinsekai_API.Migrations
{
    [DbContext(typeof(ShinsekaiApiContext))]
    [Migration("20210416040057_ShinsekaiMigration")]
    partial class ShinsekaiMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0-preview.3.21201.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Shinsekai_API.Models.AnimeArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AnimeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AnimeId");

                    b.HasIndex("ArticleId");

                    b.ToTable("AnimesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AnimeItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleItemId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleItemId");

                    b.ToTable("Animes");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BrandId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("DiscountPrice")
                        .HasColumnType("real");

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<bool>("Replica")
                        .HasColumnType("bit");

                    b.Property<string>("ShoppingCartArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BrandId")
                        .IsUnique()
                        .HasFilter("[BrandId] IS NOT NULL");

                    b.HasIndex("ShoppingCartArticleId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AuthParamItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuthParams");
                });

            modelBuilder.Entity("Shinsekai_API.Models.BrandItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Shinsekai_API.Models.CarouselItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RedirectPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Carousels");
                });

            modelBuilder.Entity("Shinsekai_API.Models.DeliveryItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EstimatedDays")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Parcel")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId")
                        .IsUnique()
                        .HasFilter("[LocationId] IS NOT NULL");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Shinsekai_API.Models.FavoriteItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ImageItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LineArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LineId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("LineId");

                    b.ToTable("LinesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LineItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("Shinsekai_API.Models.LocationItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Shinsekai_API.Models.MaterialArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaterialId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("MaterialId");

                    b.ToTable("MaterialsArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.MaterialItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Shinsekai_API.Models.OriginalItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Originals");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PointItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PromotionItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RedirectPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PruchaseId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurchaseId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("PurchaseId");

                    b.ToTable("PurchasesArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Shinsekai_API.Models.QuestionItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ReplicaItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Replicas");
                });

            modelBuilder.Entity("Shinsekai_API.Models.RequestItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurchaseId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Shinsekai_API.Models.SaleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SoldDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ShoppingCartArticleItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Article")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArticleId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ShoppingCartArticles");
                });

            modelBuilder.Entity("Shinsekai_API.Models.UserItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Admin")
                        .HasColumnType("bit");

                    b.Property<string>("AuthParamsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthParamsId")
                        .IsUnique()
                        .HasFilter("[AuthParamsId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AnimeArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.AnimeItem", "Anime")
                        .WithMany("ArticlesAnimes")
                        .HasForeignKey("AnimeId");

                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId");

                    b.Navigation("Anime");

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AnimeItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", null)
                        .WithMany("Animes")
                        .HasForeignKey("ArticleItemId");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.BrandItem", "Brand")
                        .WithOne("Article")
                        .HasForeignKey("Shinsekai_API.Models.ArticleItem", "BrandId");

                    b.HasOne("Shinsekai_API.Models.ShoppingCartArticleItem", "ShoppingCartArticle")
                        .WithMany()
                        .HasForeignKey("ShoppingCartArticleId");

                    b.Navigation("Brand");

                    b.Navigation("ShoppingCartArticle");
                });

            modelBuilder.Entity("Shinsekai_API.Models.DeliveryItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.LocationItem", "Location")
                        .WithOne("Delivery")
                        .HasForeignKey("Shinsekai_API.Models.DeliveryItem", "LocationId");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Shinsekai_API.Models.FavoriteItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("Favorites")
                        .HasForeignKey("ArticleId");

                    b.HasOne("Shinsekai_API.Models.UserItem", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId");

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
                        .WithMany()
                        .HasForeignKey("ArticleId");

                    b.HasOne("Shinsekai_API.Models.LineItem", "Line")
                        .WithMany("LinesArticles")
                        .HasForeignKey("LineId");

                    b.Navigation("Article");

                    b.Navigation("Line");
                });

            modelBuilder.Entity("Shinsekai_API.Models.MaterialArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("MaterialsArticles")
                        .HasForeignKey("ArticleId");

                    b.HasOne("Shinsekai_API.Models.MaterialItem", "Material")
                        .WithMany("Materials")
                        .HasForeignKey("MaterialId");

                    b.Navigation("Article");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("Shinsekai_API.Models.OriginalItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("Originals")
                        .HasForeignKey("ArticleId");

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PointItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.UserItem", "User")
                        .WithMany("Points")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Shinsekai_API.Models.PurchaseArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("PurchasesArticles")
                        .HasForeignKey("ArticleId");

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

            modelBuilder.Entity("Shinsekai_API.Models.ReplicaItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("Replicas")
                        .HasForeignKey("ArticleId");

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.RequestItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.PurchaseItem", "Purchase")
                        .WithMany("Requests")
                        .HasForeignKey("PurchaseId");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("Shinsekai_API.Models.SaleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.ArticleItem", "Article")
                        .WithMany("Sales")
                        .HasForeignKey("ArticleId");

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ShoppingCartArticleItem", b =>
                {
                    b.HasOne("Shinsekai_API.Models.UserItem", "User")
                        .WithMany("SoppingCartArticles")
                        .HasForeignKey("UserId");

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
                    b.Navigation("ArticlesAnimes");
                });

            modelBuilder.Entity("Shinsekai_API.Models.ArticleItem", b =>
                {
                    b.Navigation("Animes");

                    b.Navigation("Favorites");

                    b.Navigation("Images");

                    b.Navigation("MaterialsArticles");

                    b.Navigation("Originals");

                    b.Navigation("PurchasesArticles");

                    b.Navigation("Replicas");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Shinsekai_API.Models.AuthParamItem", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Shinsekai_API.Models.BrandItem", b =>
                {
                    b.Navigation("Article");
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
