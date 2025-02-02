using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class User : BaseModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string HashedPassword { get; set; }
    }
}
