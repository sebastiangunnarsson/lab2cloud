using System;
using System.Collections.Generic;

namespace Lab2.Models
{
    public class Vacation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Memory> Memories { get; set; }
    }
}