using FilmesFinal.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmesFinal.Dtos.FilmeDto
{
    public class ReadFilmeDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        [Required(ErrorMessage = "O campo Faixa Etaria é obrigatório")]
        public int FaixaEtaria { get; set; }
        public List<Genero> Generos { get; set; }
    }
}
