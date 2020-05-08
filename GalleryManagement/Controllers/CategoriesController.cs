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

namespace GalleryManagement.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly GalleryManagementDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(GalleryManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var galleryManagementDbContext = _context.Category.Include(c => c.Parent).Include(f => f.CategoryFolder);
            return View(await galleryManagementDbContext.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.Parent).Include(f => f.CategoryFolder)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            ViewData["Folders"] = new SelectList(_context.Folder.AsEnumerable(), "FolderId", "Ttitle");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVm category, string[] CategoryFolder)
        {
            if (ModelState.IsValid)
            {
                Category model = _mapper.Map<Category>(category);
                AddFolder(model, CategoryFolder);
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", category.ParentId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.Parent).Include(f => f.CategoryFolder)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            CategoryVm vm = _mapper.Map<CategoryVm>(category);
            if (category == null)
            {
                return NotFound();
            }

            ViewData["ParentId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", category.ParentId);
            vm.SelectedFolders = category.CategoryFolder.Select(e => e.FolderId).AsEnumerable();
            ViewData["SelectedFolders"] = new MultiSelectList(_context.Folder.AsEnumerable(), "FolderId", "Title", vm.SelectedFolders);
            return View(vm);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryVm category, string[] SelectedFolders)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }
            RemoveFolders(id);

            if (ModelState.IsValid)
            {
                try
                {
                    Category model = _mapper.Map<Category>(category);
                    AddFolder(model, SelectedFolders);
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            ViewData["ParentId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", category.ParentId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
        private void AddFolder(Category model, string[] CategoryFolder)
        {
            if (CategoryFolder != null)
            {
                foreach (var folder in CategoryFolder)
                {
                    model.CategoryFolder.Add(new CategoryFolder { FolderId = Convert.ToInt32(folder) });
                    _context.SaveChanges();
                }


            }
        }
        private void RemoveFolders(int id)
        {
            var folders = _context.CategoryFolder.Where(f => f.CategoryId == id).ToList();
            _context.RemoveRange(folders);
            _context.SaveChanges();


        }
    }
}
