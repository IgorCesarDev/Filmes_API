using System.ComponentModel.DataAnnotations;

namespace FilmesFinal.Data.Request
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
