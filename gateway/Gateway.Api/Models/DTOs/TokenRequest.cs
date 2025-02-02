using System.ComponentModel.DataAnnotations;

namespace Gateway.Api.Models.DTOs
{
    public class TokenRequest
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
