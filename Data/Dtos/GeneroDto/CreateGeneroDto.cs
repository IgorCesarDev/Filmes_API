using System.ComponentModel.DataAnnotations;

namespace FilmesFinal.Dtos.GeneroDto
{
    public class CreateGeneroDto
    {
        [Required(ErrorMessage = "O campo Nome é necessário")]
        public string Nome { get; set; }
    }
}
 