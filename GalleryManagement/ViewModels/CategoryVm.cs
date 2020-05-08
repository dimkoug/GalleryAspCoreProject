using GalleryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.ViewModels
{
    public class CategoryVm
    {
        public CategoryVm()
        {
            InverseParent = new HashSet<Category>();
            CategoryFolder = new HashSet<CategoryFolder>();
        }

        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }

        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> InverseParent { get; set; }
        public virtual ICollection<CategoryFolder> CategoryFolder { get; set; }
        public IEnumerable<int> SelectedFolders { get; set; }
    }
}
