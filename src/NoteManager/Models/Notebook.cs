using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteManager.Models
{
    public class Notebook
    {
        public int NotebookId { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 15)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateUtc { get; set; }

        [StringLength(450)]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}