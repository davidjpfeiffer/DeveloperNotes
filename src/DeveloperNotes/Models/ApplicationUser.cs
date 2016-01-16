using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeveloperNotes.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(35)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(35)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(71)]
        public string FullName { get; set; }

        public ICollection<Note> Notes { get; set; }

        public ICollection<Note> EditedNotes { get; set; }

        public ICollection<Revision> Revisions { get; set; }
    }
}