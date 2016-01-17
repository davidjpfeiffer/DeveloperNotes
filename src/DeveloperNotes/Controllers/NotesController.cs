using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using DeveloperNotes.Models;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;

namespace DeveloperNotes.Controllers
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
            var applicationDbContext = _context.Note.Include(n => n.Creator).ToList();

            applicationDbContext.ForEach(n => n.PublishDateUtc = n.PublishDateUtc.ToLocalTime());
            applicationDbContext.ForEach(n => n.LastEditedDateUtc = n.LastEditedDateUtc.ToLocalTime());

            return View(applicationDbContext);
        }

        // GET: Notes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult View(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Note.Include(n => n.Creator).Single(m => m.NoteId == id);
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
        public IActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {
                note.CreatorId = HttpContext.User.GetUserId();
                note.PublishDateUtc = DateTime.UtcNow;
                note.LastEditedDateUtc = DateTime.UtcNow;

                _context.Note.Add(note);
                _context.Revisions.Add(note.CreateNewRevision(HttpContext.User.GetUserId()));
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "ApplicationUser", note.CreatorId);
            return View(note);
        }

        // GET: Notes/5/Edit
        [HttpGet("{id}/Edit")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Note.Single(m => m.NoteId == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "ApplicationUser", note.CreatorId);
            return View(note);
        }

        // POST: Notes/5/Edit
        [HttpPost("{id}/Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                note.LastEditedDateUtc = DateTime.UtcNow;

                _context.Update(note);
                _context.Revisions.Add(note.CreateNewRevision(HttpContext.User.GetUserId()));
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "ApplicationUser", note.CreatorId);
            return View(note);
        }

        // GET: Notes/5/Revisions
        [HttpGet("{noteId}/Revisions")]
        public IActionResult Revisions(int? noteId)
        {
            if (noteId == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Note.Single(m => m.NoteId == noteId);

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

            Note note = this._context.Note.Single(m => m.NoteId == noteId);
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

            return RedirectToAction("View", new { id = note.NoteId });
        }

        // GET: Notes/5/Delete
        [HttpGet("{id}/Delete"), ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Note note = _context.Note.Single(m => m.NoteId == id);
            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }

        // POST: Notes/5/Delete
        [HttpPost("{id}/Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Note note = _context.Note.Include(m => m.Revisions).Single(m => m.NoteId == id);

            _context.Note.Remove(note);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
