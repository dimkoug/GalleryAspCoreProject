using GalleryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.ViewModels
{
    public class TagVm
    {

        public TagVm()
        {
            TagFolder = new HashSet<TagFolder>();
        }

        public int TagId { get; set; }
        public string Title { get; set; }
        public virtual ICollection<TagFolder> TagFolder { get; set; }
    }
}
