using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class Memory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Coordinate Position { get; set; }
        public virtual Vacation Vacation { get; set; }
        public virtual ICollection<User> TaggedUsers  { get; set; }
        public virtual ICollection<SearchTag>  SearchTags { get; set; }
        public virtual Media Media { get; set; }
    }
}