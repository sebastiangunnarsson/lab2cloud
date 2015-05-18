using System;
using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        [Required]
        public virtual Memory Memory { get; set; }
    
    }
}