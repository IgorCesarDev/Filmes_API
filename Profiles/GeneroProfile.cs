using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Dtos.GeneroDto;
using FilmesFinal.Models;
using AutoMapper;

namespace FilmesFinal.Profiles
{
    public class GeneroProfile : Profile
    {
        public GeneroProfile()
        {
            CreateMap<CreateGeneroDto, Genero>();
            CreateMap<Genero, ReadGeneroDto>();
            CreateMap<UpdateGeneroDto, Genero>();
        }
    }
}
