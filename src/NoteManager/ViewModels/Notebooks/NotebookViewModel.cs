using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteManager.Models
{
    public class NotebookViewModel
    {
        public int NotebookId { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 15)]
        public string Description { get; set; }
    }
}