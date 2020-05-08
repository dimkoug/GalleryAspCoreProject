using GalleryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Data
{
    public class GalleryManagementDbContext:DbContext
    {
        public GalleryManagementDbContext(DbContextOptions<GalleryManagementDbContext> options)
            : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryFolder> CategoryFolder { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentFolder> DocumentFolder { get; set; }

        public DbSet<Folder> Folder { get; set; }

        public DbSet<Tag> Tag { get; set; }

        public DbSet<TagFolder> TagFolder { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryFolder>()
    .HasKey(bc => new { bc.CategoryId, bc.FolderId });
            modelBuilder.Entity<DocumentFolder>()
.HasKey(bc => new { bc.DocumentId, bc.FolderId });
            modelBuilder.Entity<TagFolder>()
.HasKey(bc => new { bc.TagId, bc.FolderId });
        }
    }
}
