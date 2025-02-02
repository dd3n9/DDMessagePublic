using System.ComponentModel.DataAnnotations;

namespace Gateway.Api.Models
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }

        public Guid? ExternalUserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();    
    }
}
