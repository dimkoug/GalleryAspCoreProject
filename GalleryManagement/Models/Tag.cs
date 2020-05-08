using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Models
{
    public class Tag
    {
        public Tag()
        {
            TagFolder = new HashSet<TagFolder>();
        }

        [Key]
        public int TagId { get; set; }
        public string Title { get; set; }
        public virtual ICollection<TagFolder> TagFolder { get; set; }
    }
}
