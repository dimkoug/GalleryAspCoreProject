﻿// <auto-generated />
using System;
using GalleryManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GalleryManagement.Migrations
{
    [DbContext(typeof(GalleryManagementDbContext))]
    partial class GalleryManagementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GalleryManagement.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.HasIndex("ParentId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("GalleryManagement.Models.CategoryFolder", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId", "FolderId");

                    b.HasIndex("FolderId");

                    b.ToTable("CategoryFolder");
                });

            modelBuilder.Entity("GalleryManagement.Models.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("GalleryManagement.Models.DocumentFolder", b =>
                {
                    b.Property<int>("DocumentId")
                        .HasColumnType("int");

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.HasKey("DocumentId", "FolderId");

                    b.HasIndex("FolderId");

                    b.ToTable("DocumentFolder");
                });

            modelBuilder.Entity("GalleryManagement.Models.Folder", b =>
                {
                    b.Property<int>("FolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FolderId");

                    b.HasIndex("ParentId");

                    b.ToTable("Folder");
                });

            modelBuilder.Entity("GalleryManagement.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("GalleryManagement.Models.TagFolder", b =>
                {
                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.HasKey("TagId", "FolderId");

                    b.HasIndex("FolderId");

                    b.ToTable("TagFolder");
                });

            modelBuilder.Entity("GalleryManagement.Models.Category", b =>
                {
                    b.HasOne("GalleryManagement.Models.Category", "Parent")
                        .WithMany("InverseParent")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("GalleryManagement.Models.CategoryFolder", b =>
                {
                    b.HasOne("GalleryManagement.Models.Category", "Category")
                        .WithMany("CategoryFolder")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalleryManagement.Models.Folder", "Folder")
                        .WithMany("CategoryFolder")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GalleryManagement.Models.DocumentFolder", b =>
                {
                    b.HasOne("GalleryManagement.Models.Document", "Document")
                        .WithMany("DocumentFolder")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalleryManagement.Models.Folder", "Folder")
                        .WithMany("DocumentFolder")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GalleryManagement.Models.Folder", b =>
                {
                    b.HasOne("GalleryManagement.Models.Folder", "Parent")
                        .WithMany("InverseParent")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("GalleryManagement.Models.TagFolder", b =>
                {
                    b.HasOne("GalleryManagement.Models.Folder", "Folder")
                        .WithMany("TagFolder")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GalleryManagement.Models.Tag", "Tag")
                        .WithMany("TagFolder")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
