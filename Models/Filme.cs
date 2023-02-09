using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FilmesFinal.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public int FaixaEtaria { get; set; }
        [JsonIgnore]
        public List<Genero> Generos { get; set; }
        [JsonIgnore]
        public List<CustomIdentityUser> Usuarios { get; set; }
        public Filme()
        {

        }
    }
}
