using System.ComponentModel.DataAnnotations;

namespace Gateway.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string JwtId { get; set; }


        [Required]
        public DateTime AddedDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
