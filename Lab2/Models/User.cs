using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
        public virtual ICollection<Memory> TaggedInMemories { get; set; }
        public virtual ICollection<User> Friends  { get; set; }
        public virtual IdentityUser Account { get; set; }
    }
}