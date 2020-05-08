using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Models
{
    public class CategoryFolder
    {
        public int CategoryId { get; set; }
        public int FolderId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
