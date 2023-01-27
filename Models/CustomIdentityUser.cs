using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;

namespace FilmesFinal.Models
{
    public class CustomIdentityUser : IdentityUser<int>
    {
        public DateTime  DataNascimento { get; set; }
        public List<Filme> FilmesFavoritos { get; set; }
    }
}
