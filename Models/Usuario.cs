using System.Collections.Generic;
using System;

namespace FilmesFinal.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public List<Filme> FilmesFavoritos { get; set; }
    }
}
