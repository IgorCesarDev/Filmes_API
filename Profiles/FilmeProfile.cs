using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Models;
using AutoMapper;

namespace FilmesFinal.Profiles
{
    public class FilmeProfile : Profile
    {
        public FilmeProfile()
        {
            CreateMap<CreateFilmeDto, Filme>();
            CreateMap<Filme, ReadFilmeDto>();
            CreateMap<UpdateFilmeDto, Filme>();
        }
    }
}
