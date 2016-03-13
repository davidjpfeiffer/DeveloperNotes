using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteManager.Models
{
    public class Revision
    {
        public int RevisionId { get; set; }

        [Required]
        public int RevisionNumber { get; set; }

        public int? RestoredRevisionNumber { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishDateUtc { get; set; }

        [NotMapped]
        public string Description
        {
            get
            {
                if (this.RevisionNumber == 1)
                {
                    return "Note created by " + this.Creator.FullName;
                    //return "1";
                }
                else if (this.RestoredRevisionNumber.HasValue)
                {
                    return this.Creator.FullName + " restored Revision #" + this.RestoredRevisionNumber.Value;
                    //return "2";
                }
                else
                {
                    return "Note updated by " + this.Creator.FullName;
                    //return "3";
                }
            }
            set { }
        }

        public int NoteId { get; set; }

        public Note Note { get; set; }

        [StringLength(450)]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }
    }
}
