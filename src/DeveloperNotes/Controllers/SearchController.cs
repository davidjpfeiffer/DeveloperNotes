using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using DeveloperNotes.Models;
using Microsoft.Data.Entity;

namespace DeveloperNotes.Controllers
{
    [Route("[controller]")]
    [RequireHttps]
    public class SearchController : Controller
    {
        private ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Search
        [HttpGet]
        public IActionResult Index(string q)
        {
            List<Note> results;

            if (String.IsNullOrEmpty(q))
            {
                results = _context.Notes.Include(n => n.Creator).ToList();
            }
            else
            {
                results = _context.Notes.Where(n => n.Title.Contains(q)).Include(n => n.Creator).ToList();
            }

            results.ForEach(n => n.PublishDateUtc = n.PublishDateUtc.ToLocalTime());
            results.ForEach(n => n.LastEditedDateUtc = n.LastEditedDateUtc.ToLocalTime());
            results.Reverse();

            return View(results);
        }
    }
}