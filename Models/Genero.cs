using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FilmesFinal.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [JsonIgnore]
        public List<Filme> Filmes { get; set; }
        public Genero()
        {

        }
    }
}
