using System;
using System.ComponentModel.DataAnnotations;

namespace DeveloperNotes.Models
{
    public class Revision
    {
        public int RevisionId { get; set; }

        [Required]
        public int RevisionNumber { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Published")]
        [DataType(DataType.Date)]
        public DateTime PublishDateUtc { get; set; }

        public int NoteId { get; set; }

        public Note Note { get; set; }

        [StringLength(450)]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }
    }
}
