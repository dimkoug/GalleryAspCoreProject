using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GalleryManagement.Data;
using GalleryManagement.Models;
using AutoMapper;
using GalleryManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;

namespace GalleryManagement.Controllers
{
    public class FoldersController : Controller
    {
        private readonly GalleryManagementDbContext _context;
        private readonly IMapper _mapper;

        public FoldersController(GalleryManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Folders
        public async Task<IActionResult> Index()
        {
            var galleryManagementDbContext = _context.Folder.Include(f => f.Parent).Include(f=> f.CategoryFolder).Include(f=>f.TagFolder);
            return View(await galleryManagementDbContext.ToListAsync());
        }

        // GET: Folders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folder
                .Include(f => f.Parent).Include(f => f.Parent).Include(f => f.CategoryFolder).Include(f => f.TagFolder)
                .FirstOrDefaultAsync(m => m.FolderId == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // GET: Folders/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Folder, "FolderId", "Title");
            ViewData["Categories"] = new SelectList(_context.Category.AsEnumerable(), "CategoryId", "Title");
            ViewData["Tags"] = new SelectList(_context.Tag.AsEnumerable(), "TagId", "Title");
            return View();
        }

        // POST: Folders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FolderVm folder, string[] CategoryFolder, string[] TagFolder, List<IFormFile> Files)
        {
            if (ModelState.IsValid)
            {
                Folder model = _mapper.Map<Folder>(folder);
                AddCategories(model, CategoryFolder);
                AddTags(model, TagFolder);
                AddFiles(model, Files);
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Folder, "FolderId", "FolderId", folder.ParentId);
            return View(folder);
        }

        // GET: Folders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folder.Include(f => f.Parent).Include(f => f.CategoryFolder).Include(f => f.TagFolder).FirstOrDefaultAsync(i => i.FolderId == id);
            FolderVm vm = _mapper.Map<FolderVm>(folder);
            if (folder == null)
            {
                return NotFound();
            }
            vm.SelectedCategories = folder.CategoryFolder.Select(e => e.CategoryId).AsEnumerable();
            vm.SelectedTags = folder.TagFolder.Select(e => e.TagId).AsEnumerable();
            ViewData["ParentId"] = new SelectList(_context.Folder, "FolderId", "Title", folder.ParentId);
            ViewData["SelectedCategories"] = new MultiSelectList(_context.Category.AsEnumerable(), "CategoryId", "Title", vm.CategoryFolder.Select(c => c.CategoryId));
            ViewData["SelectedTags"] = new MultiSelectList(_context.Tag.AsEnumerable(), "TagId", "Title", vm.TagFolder.Select(c => c.TagId));
            return View(vm);
        }

        // POST: Folders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FolderVm folder, string[] SelectedCategories, string[] SelectedTags, List<IFormFile> Files)
        {
            if (id != folder.FolderId)
            {
                return NotFound();
            }
            RemoveCategories(id);
            RemoveFiles(id);
            RemoveTags(id);

            if (ModelState.IsValid)
            {
                try
                {
                    Folder model = _mapper.Map<Folder>(folder);

                    AddCategories(model, SelectedCategories);
                    AddTags(model, SelectedTags);
                    AddFiles(model, Files);
    
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolderExists(folder.FolderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Folder, "FolderId", "FolderId", folder.ParentId);
            return View(folder);
        }

        // GET: Folders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folder
                .Include(f => f.Parent)
                .FirstOrDefaultAsync(m => m.FolderId == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var folder = await _context.Folder.FindAsync(id);
            _context.Folder.Remove(folder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FolderExists(int id)
        {
            return _context.Folder.Any(e => e.FolderId == id);
        }


        private void AddFiles(Folder model, List<IFormFile> Files)
        {
            if (Files != null && Files.Count > 0)
            {
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploaded_files");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                foreach (IFormFile item in Files)
                {
                    if (item.Length > 0)
                    {
                        string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                        // If file with same name exists delete it
                        string fullPath = Path.Combine(newPath, fileName);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }
                        var document = new Document { FilePath = item.FileName };
                        _context.Add(document);
                        _context.SaveChanges();
                        model.DocumentFolder.Add(new DocumentFolder { DocumentId = document.DocumentId });
                    }
                }
            }

        }
        private void AddCategories(Folder model, string[] CategoryFolder)
        {
            if (CategoryFolder != null)
            {
                foreach (var category in CategoryFolder)
                {
                    model.CategoryFolder.Add(new CategoryFolder { CategoryId = Convert.ToInt32(category) });
                    _context.SaveChanges();
                }


            }
        }
        private void AddTags(Folder model, string[] TagFolder)
        {
            if (TagFolder != null)
            {
                foreach (var tag in TagFolder)
                {
                    model.TagFolder.Add(new TagFolder { TagId = Convert.ToInt32(tag) });
                    _context.SaveChanges();
                }


            }
        }

        private void RemoveFiles(int id)
        {
            var files = _context.DocumentFolder.Where(f => f.FolderId == id).ToList();
            _context.RemoveRange(files);
            _context.SaveChanges();
        }

        private void RemoveCategories(int id)
        {
            var categories = _context.CategoryFolder.Where(f => f.FolderId == id).ToList();
            _context.RemoveRange(categories);
            _context.SaveChanges();
        }

        private void RemoveTags(int id)
        {
            var tags = _context.TagFolder.Where(f => f.FolderId == id).ToList();
            _context.RemoveRange(tags);
            _context.SaveChanges();
        }
    }
}
