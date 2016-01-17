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
        public int NumberOfRevisions { get; set; }

        public ICollection<Revision> Revisions { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishDateUtc { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastEditedDateUtc { get; set; }

        [StringLength(450)]
        public string CreatorId{ get; set; }

        public ApplicationUser Creator { get; set; }

        public Revision CreateNewRevision(string creatorId, int? restoredRevisionNumber = null)
        {
            Revision newRevision = new Revision();
            newRevision.CreatorId = creatorId;
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
