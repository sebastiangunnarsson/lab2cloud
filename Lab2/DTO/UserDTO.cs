using System.ComponentModel.DataAnnotations;

namespace Lab2.Controllers.V1
{
    public class UserDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }
    }
}