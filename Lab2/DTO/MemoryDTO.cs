using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lab2.Models;

namespace Lab2.Controllers.V1
{
    public class MemoryDTO
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public Coordinate Position { get; set; }

        [Required]
        public List<string> TaggedUsers { get; set; }

        [Required]
        public List<string> SearchTags { get; set; }
    }
}