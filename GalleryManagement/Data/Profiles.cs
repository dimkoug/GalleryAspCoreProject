using AutoMapper;
using GalleryManagement.Models;
using GalleryManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryManagement.Data
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<CategoryVm, Category>().ReverseMap();
            CreateMap<DocumentVm, Document>().ReverseMap();
            CreateMap<FolderVm, Folder>().ReverseMap();
            CreateMap<TagVm, Tag>().ReverseMap();
        }
    }
}
