using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class BaseModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
