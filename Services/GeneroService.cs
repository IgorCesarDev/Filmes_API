using FilmesFinal.Data;
using FilmesFinal.Dtos.GeneroDto;
using FilmesFinal.Models;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FilmesFinal.Services
{
    public class GeneroService
    {
        private UserDbContext _context;
        private IMapper _mapper;
        private IMemoryCache _memoryCache;
        private const string GENERO_KEY = "Generos";
        MemoryCacheEntryOptions memoryCachEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
            SlidingExpiration = TimeSpan.FromSeconds(1200)
        };
        public GeneroService(UserDbContext context, IMapper mapper, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<List<ReadGeneroDto>> RecuperaGeneros()
        {
            if(_memoryCache.TryGetValue(GENERO_KEY, out List<Genero>generos))
            {
                List<ReadGeneroDto> generosDto = _mapper.Map<List<ReadGeneroDto>>(generos);
                return generosDto;
            }
            generos = await _context.Generos.Include(p => p.Filmes).ToListAsync();
            if(generos != null)
            {
                _memoryCache.Set(GENERO_KEY, generos, memoryCachEntryOptions);
                List<ReadGeneroDto> generosDto =_mapper.Map<List<ReadGeneroDto>>(generos);
                return generosDto;
            }
            return null;
        }

        public async Task<ReadGeneroDto> AdicionaGenero(CreateGeneroDto generoDto)
        {
            Genero genero = _mapper.Map<Genero>(generoDto);
            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadGeneroDto>(genero);
        }

        public async Task<ReadGeneroDto> RecuperaGenerosPorId(int id)
        {
            if (_memoryCache.TryGetValue(GENERO_KEY, out List<Genero> generos))
            {
                var generoSelecionado = generos.FirstOrDefault(g => g.Id == id);
                ReadGeneroDto generoSelecionadoDto = _mapper.Map<ReadGeneroDto>(generoSelecionado);
                return generoSelecionadoDto;
            }
            Genero genero = await _context.Generos.Include(f => f.Filmes).FirstOrDefaultAsync(g => g.Id == id);
            if (genero == null) { return null; }
            return _mapper.Map<ReadGeneroDto>(genero);
        }

        public async Task<Result> AtualizaGeneros(int id, UpdateGeneroDto generoDto)
        {
            Genero genero = _context.Generos.FirstOrDefault(g => g.Id == id);
            if (genero == null) { return Result.Fail("Erro, Genero não encontrado"); }
            _mapper.Map(generoDto, genero);
            await _context.SaveChangesAsync();       
            if(_memoryCache.TryGetValue(GENERO_KEY, out List<Genero> generos))
            {
                _memoryCache.Remove(GENERO_KEY);
                _memoryCache.Set(GENERO_KEY, generos, memoryCachEntryOptions);
            }
            return Result.Ok();
        }
        public async Task<Result> DeletaGenero(int id)
        {
            Genero genero = await _context.Generos.FirstOrDefaultAsync(g => g.Id==id);
            if (genero == null) { return Result.Fail("Erro, Genero não encontrado"); }
            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();
            if (_memoryCache.TryGetValue(GENERO_KEY, out List<Genero> generos))
            {
                generos.Remove(genero);
                _memoryCache.Set(GENERO_KEY, generos, memoryCachEntryOptions);
            }
            return Result.Ok();

        }
    }
}
