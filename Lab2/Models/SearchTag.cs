using System.Collections.Generic;

namespace Lab2.Models
{
    public class SearchTag
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public virtual ICollection<Memory> Memories { get; set; }
    }
}