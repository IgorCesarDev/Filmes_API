using FilmesFinal.Dtos.UsuarioDto;
using FilmesFinal.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace FilmesFinal.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile() 
        {
            CreateMap<CreateUsuarioDto, Usuario>();
            CreateMap<Usuario, IdentityUser<int>>();
            CreateMap<Usuario, CustomIdentityUser>();
        }

    }
}
