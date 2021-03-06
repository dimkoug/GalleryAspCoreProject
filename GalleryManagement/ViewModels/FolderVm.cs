﻿using GalleryManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.ViewModels
{
    public class FolderVm
    {
        public FolderVm()
        {
            InverseParent = new HashSet<Folder>();
            CategoryFolder = new HashSet<CategoryFolder>();
            DocumentFolder = new HashSet<DocumentFolder>();
            TagFolder = new HashSet<TagFolder>();
        }

        public int FolderId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public virtual Folder Parent { get; set; }
        public virtual ICollection<Folder> InverseParent { get; set; }
        public virtual ICollection<CategoryFolder> CategoryFolder { get; set; }
        public virtual ICollection<DocumentFolder> DocumentFolder { get; set; }

        public virtual ICollection<TagFolder> TagFolder { get; set; }

        public IEnumerable<int> SelectedCategories { get; set; }
        public IEnumerable<int> SelectedDocuments { get; set; }
        public IEnumerable<int> SelectedTags { get; set; }

        [DisplayName("Έγγραφα")]
        public IFormFileCollection Files { get; set; }
    }
}
