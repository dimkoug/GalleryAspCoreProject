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

namespace GalleryManagement.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly GalleryManagementDbContext _context;
        private readonly IMapper _mapper;

        public DocumentsController(GalleryManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Document.Include(f => f.DocumentFolder).ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(f => f.DocumentFolder).FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["SelectedFolders"] = new SelectList(_context.Folder.AsEnumerable(), "FolderId", "Title");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentVm document, string[] DocumentFolder, IFormFile File)
        {
            if (ModelState.IsValid)
            {
                Document model = _mapper.Map<Document>(document);
                AddFile(model, File);
                AddFolder(model, DocumentFolder);
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Document document = await _context.Document.Include(f => f.DocumentFolder).FirstOrDefaultAsync(m => m.DocumentId == id);
            DocumentVm vm = _mapper.Map<DocumentVm>(document);
            if (document == null)
            {
                return NotFound();
            }
            vm.SelectedFolders = document.DocumentFolder.Select(e => e.FolderId).AsEnumerable();
            ViewData["SelectedFolders"] = new MultiSelectList(_context.Folder.AsEnumerable(), "FolderId", "Title", vm.SelectedFolders);
            return View(vm);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentVm document , string[] SelectedFolders, IFormFile File)
        {
            if (id != document.DocumentId)
            {
                return NotFound();
            }
            RemoveFolders(id);

            if (ModelState.IsValid)
            {
                try
                {
                    Document model = _mapper.Map<Document>(document);
                    
                    AddFile(model, File);
                    AddFolder(model, SelectedFolders);
     
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentId))
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
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.DocumentId == id);
        }

        private async void AddFile(Document model, IFormFile File)
        {
            if (File != null && File.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploaded_files", File.FileName);

                // If file with same name exists delete it
                if (System.IO.File.Exists(File.FileName))
                {
                    System.IO.File.Delete(File.FileName);
                }

                // Create new local file and copy contents of uploaded file
                //using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = new FileStream(filePath, FileMode.Create))
                {
                    await File.CopyToAsync(uploadedFile);
                }


                model.FilePath = File.FileName;
            }
        }
        private void AddFolder(Document model, string[] DocumentFolder)
        {
            if (DocumentFolder != null)
            {
                foreach (var folder in DocumentFolder)
                {
                    model.DocumentFolder.Add(new DocumentFolder { FolderId = Convert.ToInt32(folder) });
                    _context.SaveChanges();
                }


            }
        }

        private void RemoveFolders(int id)
        {
            var folders = _context.DocumentFolder.Where(f => f.DocumentId == id).ToList();
            _context.RemoveRange(folders);
            _context.SaveChanges();
            

        }

    }
}
