using System.ComponentModel.DataAnnotations;

namespace FilmesFinal.Request
{
    public class SolicitaResetRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
