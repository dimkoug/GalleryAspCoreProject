using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Models
{
    public class Folder
    {
        public Folder()
        {
            InverseParent = new HashSet<Folder>();
            CategoryFolder = new HashSet<CategoryFolder>();
            DocumentFolder = new HashSet<DocumentFolder>();
            TagFolder = new HashSet<TagFolder>();
        }

        [Key]
        public int FolderId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public virtual Folder Parent { get; set; }
        public virtual ICollection<Folder> InverseParent { get; set; }
        public virtual ICollection<CategoryFolder> CategoryFolder { get; set; }
        public virtual ICollection<DocumentFolder> DocumentFolder { get; set; }

        public virtual ICollection<TagFolder> TagFolder { get; set; }
    }
}
