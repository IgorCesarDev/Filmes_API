using FilmesFinal.Models;
using System.Collections.Generic;

namespace FilmesFinal.Dtos.GeneroDto
{
    public class ReadGeneroDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Filme> Filmes { get; set; }

    }
}
