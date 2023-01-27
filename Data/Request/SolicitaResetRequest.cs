using System.ComponentModel.DataAnnotations;

namespace FilmesFinal.Data.Request
{
    public class SolicitaResetRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
