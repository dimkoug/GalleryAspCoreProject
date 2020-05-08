using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Models
{
    public class Category
    {
        public Category()
        {
            InverseParent = new HashSet<Category>();
            CategoryFolder = new HashSet<CategoryFolder>();
        }

        [Key]
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> InverseParent { get; set; }
        public virtual ICollection<CategoryFolder> CategoryFolder { get; set; }
    }
}
