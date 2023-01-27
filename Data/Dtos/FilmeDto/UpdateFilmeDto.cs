using System.ComponentModel.DataAnnotations;

namespace FilmesFinal.Dtos.FilmeDto
{
    public class UpdateFilmeDto
    {
        [Required(ErrorMessage = "O Campo Título é obrigatório")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "O Campo SubTítulo é obrigatório")]
        public string SubTitulo { get; set; }
        [Required(ErrorMessage = "O campo Faixa Etaria é obrigatório")]
        public int FaixaEtaria { get; set; }
    }
}
