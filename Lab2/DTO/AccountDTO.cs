using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class AccountDTO
    {
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [StringLength(100,ErrorMessage="The password must be at least 6 characters long",MinimumLength = 6)]
        [Required]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage="Passwords doesn't match")]
        [DataType(DataType.Password)]
        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
