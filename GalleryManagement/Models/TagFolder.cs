using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Models
{
    public class TagFolder
    {
        public int TagId { get; set; }
        public int FolderId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
