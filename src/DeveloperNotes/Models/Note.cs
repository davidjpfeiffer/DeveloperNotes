using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeveloperNotes.Models
{
    public class Note
    {
        public int NoteId { get; set; }
        
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Published")]
        [DataType(DataType.Date)]
        public DateTime PublishDateUtc { get; set; }

        [Required]
        [Display(Name = "Last Edited")]
        [DataType(DataType.Date)]
        public DateTime LastEditedDateUtc { get; set; }

        [Required]
        public int NumberOfRevisions { get; set; }

        public ICollection<Revision> Revisions { get; set; }

        [StringLength(450)]
        public string CreatorId{ get; set; }

        public ApplicationUser Creator { get; set; }

        [StringLength(450)]
        public string LastEditedByUserId { get; set; }

        public ApplicationUser LastEditedByUser { get; set; }

        public Revision CreateNewRevision(int? restoredRevisionNumber = null)
        {
            Revision newRevision = new Revision();
            newRevision.CreatorId = this.LastEditedByUserId == null ? this.CreatorId : this.LastEditedByUserId;
            newRevision.Content = this.Content;
            newRevision.NoteId = this.NoteId;
            newRevision.PublishDateUtc = this.PublishDateUtc;
            newRevision.Title = this.Title;
            newRevision.RevisionNumber = ++this.NumberOfRevisions;
            newRevision.RestoredRevisionNumber = restoredRevisionNumber;

            return newRevision;
        }
    }
}
