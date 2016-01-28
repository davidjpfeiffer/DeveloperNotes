using DeveloperNotes.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DeveloperNotes.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [RequireHttps]
    public class NotebooksController : Controller
    {
        private ApplicationDbContext _context;

        public NotebooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notebooks
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Notebook> notebooks = _context.Notebooks.Include(n => n.Creator).ToList();
            notebooks.ForEach(n => n.CreatedDateUtc = n.CreatedDateUtc.ToLocalTime());
            notebooks.Reverse();

            return View(notebooks);
        }

        // GET: Notebooks/5
        [HttpGet("{notebookId}")]
        [AllowAnonymous]
        public IActionResult View(int? notebookId)
        {
            if (notebookId == null)
            {
                return HttpNotFound();
            }

            Notebook notebook = _context.Notebooks.Include(n => n.Creator).Include(n => n.Notes).Single(m => m.NotebookId == notebookId);

            if (notebook == null)
            {
                return HttpNotFound();
            }

            notebook.CreatedDateUtc = notebook.CreatedDateUtc.ToLocalTime();

            return View(notebook);
        }

        // GET: Notebooks/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Creator");
            return View();
        }

        // POST: Notebooks/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NotebookViewModel notebookViewModel)
        {
            if (ModelState.IsValid)
            {
                Notebook notebook = new Notebook();
                notebook.Name = notebookViewModel.Name;
                notebook.Description = notebookViewModel.Description;
                notebook.CreatedDateUtc = DateTime.UtcNow;
                notebook.CreatorId = HttpContext.User.GetUserId();

                _context.Notebooks.Add(notebook);
                _context.SaveChanges();

                ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Creator", notebook.CreatorId);
                return RedirectToAction("View", new { notebookId = notebook.NotebookId });
            }

            return HttpBadRequest();
        }

        // GET: Notebooks/5/Edit
        [HttpGet("{notebookId}/Edit")]
        public IActionResult Edit(int? notebookId)
        {
            if (notebookId == null)
            {
                return HttpNotFound();
            }

            Notebook notebook = _context.Notebooks.Single(m => m.NotebookId == notebookId);

            if (notebook == null)
            {
                return HttpNotFound();
            }

            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Creator", notebook.CreatorId);
            return View(notebook);
        }

        // POST: Notebooks/5/Edit
        [HttpPost("{notebookId}/Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NotebookViewModel notebookViewModel)
        {
            if (ModelState.IsValid)
            {
                Notebook notebook = _context.Notebooks.Single(n => n.NotebookId == notebookViewModel.NotebookId);
                notebook.Name = notebookViewModel.Name;
                notebook.Description = notebookViewModel.Description;

                _context.Update(notebook);
                _context.SaveChanges();

                ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Creator", notebook.CreatorId);
                return RedirectToAction("View", new { notebookId = notebook.NotebookId });
            }

            return HttpBadRequest();
        }

        // GET: Notesbooks/5/AddNote
        [HttpGet("{notebookId}/Add")]
        public IActionResult ViewNotesToAdd(int? notebookId, string q)
        {
            if(notebookId.HasValue)
            {
                List<Note> notes;

                if (String.IsNullOrEmpty(q))
                {
                    notes = _context.Notes.Include(n => n.Creator).ToList();
                }
                else
                {
                    notes = _context.Notes.Include(n => n.Creator).Where(n => n.Title.Contains(q)).ToList();
                }

                notes.ForEach(n => n.PublishDateUtc = n.PublishDateUtc.ToLocalTime());
                notes.ForEach(n => n.LastEditedDateUtc = n.LastEditedDateUtc.ToLocalTime());
                notes.Reverse();

                ViewData["Query"] = q;
                return View(notes);
            }

            return HttpBadRequest();
        }

        // GET: Notesbooks/5/AddNote/6
        [HttpGet("{notebookId}/Add/{noteId}")]
        public IActionResult AddNote(int? notebookId, int? noteId)
        {
            if (notebookId.HasValue && noteId.HasValue)
            {
                Note note = _context.Notes.Include(n => n.Creator).Single(n => n.NoteId == noteId);
                Notebook notebook = _context.Notebooks.Single(n => n.NotebookId == notebookId);

                if (note != null && notebook != null)
                {
                    return View(note);
                }
            }

            return HttpBadRequest();
        }

        // POST: Notesbooks/5/AddNote/6
        [HttpPost("{notebookId}/Add/{noteId}")]
        public IActionResult AddNoteConfirmed(int? notebookId, int? noteId)
        {
            if (notebookId.HasValue && noteId.HasValue)
            {
                Note note = _context.Notes.Include(n => n.Creator).Single(n => n.NoteId == noteId);
                Notebook notebook = _context.Notebooks.Include(n => n.Notes).Single(n => n.NotebookId == notebookId);

                if (note != null && notebook != null)
                {
                    notebook.Notes.Add(note);
                    _context.SaveChanges();
                    return RedirectToAction("View", new { notebookId = notebook.NotebookId });
                }
            }

            return HttpBadRequest();
        }

        // GET: Notebooks/5/Delete
        [HttpGet("{notebookId}/Delete"), ActionName("Delete")]
        public IActionResult Delete(int? notebookId)
        {
            if (notebookId == null)
            {
                return HttpNotFound();
            }

            Notebook notebook = _context.Notebooks.Include(n => n.Creator).Single(m => m.NotebookId == notebookId);

            if (notebook == null)
            {
                return HttpNotFound();
            }

            return View(notebook);
        }

        // POST: Notebooks/5/Delete
        [HttpPost("{notebookId}/Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int notebookId)
        {
            Notebook notebook = _context.Notebooks.Single(m => m.NotebookId == notebookId);

            _context.Notebooks.Remove(notebook);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
