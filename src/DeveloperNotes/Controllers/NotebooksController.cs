using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using DeveloperNotes.Models;

namespace DeveloperNotes.Controllers
{
    public class NotebooksController : Controller
    {
        private ApplicationDbContext _context;

        public NotebooksController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Notebooks
        public IActionResult Index()
        {
            return View(_context.Notebook.ToList());
        }

        // GET: Notebooks/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Notebook notebook = _context.Notebook.Single(m => m.NotebookId == id);
            if (notebook == null)
            {
                return HttpNotFound();
            }

            return View(notebook);
        }

        // GET: Notebooks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notebooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Notebook notebook)
        {
            if (ModelState.IsValid)
            {
                _context.Notebook.Add(notebook);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notebook);
        }

        // GET: Notebooks/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Notebook notebook = _context.Notebook.Single(m => m.NotebookId == id);
            if (notebook == null)
            {
                return HttpNotFound();
            }
            return View(notebook);
        }

        // POST: Notebooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Notebook notebook)
        {
            if (ModelState.IsValid)
            {
                _context.Update(notebook);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notebook);
        }

        // GET: Notebooks/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Notebook notebook = _context.Notebook.Single(m => m.NotebookId == id);
            if (notebook == null)
            {
                return HttpNotFound();
            }

            return View(notebook);
        }

        // POST: Notebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Notebook notebook = _context.Notebook.Single(m => m.NotebookId == id);
            _context.Notebook.Remove(notebook);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
