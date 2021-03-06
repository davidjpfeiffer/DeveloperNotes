using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using NoteManager.Models;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;

namespace NoteManager.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [RequireHttps]
    public class NotesController : Controller
    {
        private ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notes
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Note> notes = _context.Notes.Include(n => n.Creator).ToList();

            notes.ForEach(n => n.PublishDateUtc = n.PublishDateUtc.ToLocalTime());
            notes.ForEach(n => n.LastEditedDateUtc = n.LastEditedDateUtc.ToLocalTime());
            notes.Reverse();

            return View(notes);
        }

        // GET: Notes/Search
        [HttpGet("Search")]
        [AllowAnonymous]
        public IActionResult Search(string q)
        {
            List<Note> notes;

            if (String.IsNullOrEmpty(q))
            {
                notes = new List<Note>();
            }
            else
            {
                string[] queries = q.Split(' ', '-');
                notes = _context.Notes.Include(n => n.Creator).Where(n => customContains(n.Title, queries)).ToList();
            }

            notes.ForEach(n => n.PublishDateUtc = n.PublishDateUtc.ToLocalTime());
            notes.ForEach(n => n.LastEditedDateUtc = n.LastEditedDateUtc.ToLocalTime());
            notes.Reverse();

            ViewData["Query"] = q;
            return View(notes);
        }

        // GET: Notes/5
        [HttpGet("{noteId}")]
        [AllowAnonymous]
        public IActionResult View(int? noteId)
        {
            if (noteId == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Notes.Include(n => n.Creator).Single(m => m.NoteId == noteId);
            if (note == null)
            {
                return HttpNotFound();
            }

            note.PublishDateUtc = note.PublishDateUtc.ToLocalTime();
            note.LastEditedDateUtc = note.LastEditedDateUtc.ToLocalTime();

            return View(note);
        }

        // GET: Notes/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "ApplicationUser");
            return View();
        }

        // POST: Notes/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoteViewModel noteViewModel)
        {
            if (ModelState.IsValid)
            {
                Note newNote = new Note();

                newNote.Title = noteViewModel.Title;
                newNote.Content = noteViewModel.Content;
                newNote.CreatorId = HttpContext.User.GetUserId();
                newNote.PublishDateUtc = DateTime.UtcNow;
                newNote.LastEditedDateUtc = DateTime.UtcNow;

                _context.Notes.Add(newNote);
                _context.Revisions.Add(newNote.CreateNewRevision(HttpContext.User.GetUserId()));
                _context.SaveChanges();

                ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "ApplicationUser", newNote.CreatorId);
                return RedirectToAction("View", new { noteId = newNote.NoteId });
            }
            return HttpBadRequest();
        }

        // GET: Notes/5/Edit
        [HttpGet("{noteId}/Edit")]
        public IActionResult Edit(int? noteId)
        {
            if (noteId == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Notes.Single(m => m.NoteId == noteId);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "ApplicationUser", note.CreatorId);
            return View(note);
        }

        // POST: Notes/5/Edit
        [HttpPost("{noteId}/Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoteViewModel noteViewModel)
        {
            if (ModelState.IsValid)
            {
                Note existingNote = _context.Notes.Single(n => n.NoteId == noteViewModel.NoteId);

                existingNote.Title = noteViewModel.Title;
                existingNote.Content = noteViewModel.Content;
                existingNote.LastEditedDateUtc = DateTime.UtcNow;

                _context.Update(existingNote);
                _context.Revisions.Add(existingNote.CreateNewRevision(HttpContext.User.GetUserId()));
                _context.SaveChanges();

                ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "ApplicationUser", existingNote.CreatorId);
                return RedirectToAction("View", new { noteId = existingNote.NoteId });
            }
            return HttpBadRequest();
        }

        // GET: Notes/5/Revisions
        [HttpGet("{noteId}/Revisions")]
        public IActionResult Revisions(int? noteId)
        {
            if (noteId == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Notes.Single(m => m.NoteId == noteId);

            if (note == null)
            {
                return HttpNotFound();
            }

            List<Revision> revisions = _context.Revisions.Include(n => n.Creator).Include(n => n.Note).Where(m => m.NoteId == note.NoteId).ToList();
            revisions.ForEach(n => n.PublishDateUtc = n.PublishDateUtc.ToLocalTime());

            return View(revisions);
        }

        // GET: Notes/5/Revision/6
        [HttpGet("{noteId}/Revision/{revisionNumber}")]
        public IActionResult Revision(int? noteId, int? revisionNumber)
        {
            if (noteId == null || revisionNumber == null)
            {
                return HttpNotFound();
            }

            Revision revision = this._context.Revisions.Include(m => m.Creator).Single(m => m.NoteId == noteId && m.RevisionNumber == revisionNumber);

            if (revision == null)
            {
                return HttpNotFound();
            }

            return View(revision);
        }

        // GET: Notes/5/Revision/6
        [HttpGet("{noteId}/Restore/{revisionNumber}")]
        public IActionResult Restore(int? noteId, int? revisionNumber)
        {
            if (noteId == null || revisionNumber == null)
            {
                return HttpNotFound();
            }

            Revision revision = this._context.Revisions.Include(m => m.Creator).Single(m => m.NoteId == noteId && m.RevisionNumber == revisionNumber);

            if (revision == null)
            {
                return HttpNotFound();
            }

            return View(revision);
        }

        // POST: Notes/5/Revision/6
        [HttpPost("{noteId}/Restore/{revisionNumber}")]
        [ValidateAntiForgeryToken]
        public IActionResult RestoreConfirmed(int? noteId, int? revisionNumber)
        {
            if (noteId == null || revisionNumber == null)
            {
                return HttpNotFound();
            }

            Note note = this._context.Notes.Single(m => m.NoteId == noteId);
            Revision revision = this._context.Revisions.Single(m => m.NoteId == noteId && m.RevisionNumber == revisionNumber);

            if (note == null || revision == null)
            {
                return HttpNotFound();
            }

            note.LastEditedDateUtc = DateTime.UtcNow;
            note.Content = revision.Content;
            note.Title = revision.Title;

            _context.Revisions.Add(note.CreateNewRevision(HttpContext.User.GetUserId(), revisionNumber));
            _context.SaveChanges();

            return RedirectToAction("View", new { noteId = note.NoteId });
        }

        // GET: Notes/5/Delete
        [HttpGet("{noteId}/Delete"), ActionName("Delete")]
        public IActionResult Delete(int? noteId)
        {
            if (noteId == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Notes.Single(m => m.NoteId == noteId);

            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }

        // POST: Notes/5/Delete
        [HttpPost("{noteId}/Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int noteId)
        {
            Note note = _context.Notes.Include(m => m.Revisions).Single(m => m.NoteId == noteId);

            _context.Notes.Remove(note);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool customContains(string title, string[] queries)
        {
            foreach(string query in queries)
            {
                if (title.ToLower().Contains(query.ToLower())) return true;
            }
            return false;
        }
    }
}