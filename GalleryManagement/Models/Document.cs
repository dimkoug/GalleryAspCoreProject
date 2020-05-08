using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Models
{
    public class Document
    {
        public Document()
        {
            DocumentFolder = new HashSet<DocumentFolder>();
        }

        [Key]
        public int DocumentId { get; set; }
        public string FilePath { get; set; }
        public virtual ICollection<DocumentFolder> DocumentFolder { get; set; }
    }
}
