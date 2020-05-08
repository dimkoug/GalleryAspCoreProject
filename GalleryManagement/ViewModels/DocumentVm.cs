using GalleryManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.ViewModels
{
    public class DocumentVm
    {
        public DocumentVm()
        {
            DocumentFolder = new HashSet<DocumentFolder>();
        }


        public int DocumentId { get; set; }
        public string FilePath { get; set; }
        public virtual ICollection<DocumentFolder> DocumentFolder { get; set; }
        public IEnumerable<int> SelectedFolders { get; set; }
        [DisplayName("Έγγραφα")]
        public IFormFile File { get; set; }

    }
}
