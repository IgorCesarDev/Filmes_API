using System.ComponentModel.DataAnnotations;

namespace FilmesFinal.Data.Request
{
    public class AtivaContaRequest
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public string CodigoDeAtivacao { get; set; }
    }
}
