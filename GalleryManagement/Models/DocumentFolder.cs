using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Models
{
    public class DocumentFolder
    {
        public int DocumentId { get; set; }
        public int FolderId { get; set; }

        public virtual Document Document { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
