using FilmesFinal.Data;
using FilmesFinal.Dtos.FilmeDto;
using FilmesFinal.Models;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata;
using System;
using System.Threading.Tasks;

namespace FilmesFinal.Services
{
    public class FilmeService
    {
        private UserDbContext _context;
        private IMapper _mapper;
        private IMemoryCache _memoryCache;
        private const string FILMES_KEY = "Filmes";
        MemoryCacheEntryOptions memoryCachEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
            SlidingExpiration = TimeSpan.FromSeconds(1200)
        };
        public FilmeService(UserDbContext context, IMapper mapper, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<ReadFilmeDto> AdicionaFilme(CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);
            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReadFilmeDto>(filme);
        }
        public async Task<List<ReadFilmeDto>> RecuperaFilmesAsync(int skip, int take)
        {
            if (_memoryCache.TryGetValue(FILMES_KEY, out List<Filme> filmes))
            {
                List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(filmes);
                return filmesDto;
            }
            filmes = await _context.Filmes.Include(f => f.Generos).Skip(skip).Take(take).ToListAsync();
            if (filmes != null)
            {
                _memoryCache.Set(FILMES_KEY, filmes, memoryCachEntryOptions);
                List<ReadFilmeDto> filmesDto = _mapper.Map<List<ReadFilmeDto>>(filmes);
                return filmesDto;
            }
            return null;
        }
        public async Task<ReadFilmeDto> RecuperaFilmesPorIdAsync(int id)
        {
            if (_memoryCache.TryGetValue(FILMES_KEY, out List<Filme> filmes))
            {
                var filmeSelecionado = filmes.FirstOrDefault(f => f.Id == id);
                ReadFilmeDto filmeSelecionadoDto = _mapper.Map<ReadFilmeDto>(filmeSelecionado);
                return filmeSelecionadoDto;
            }
            Filme filme = await _context.Filmes.Include(p => p.Generos).FirstOrDefaultAsync(p => p.Id == id);
            if (filme != null)
            {
                ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
                return filmeDto;
            }
            return null;
        }
        public async Task<Result> AtualizaFilme(int id, UpdateFilmeDto filmeDto)
        {
            Filme filme = await _context.Filmes.FirstOrDefaultAsync(filme => filme.Id == id);
            if (filme == null) {return Result.Fail("Erro, Filme não encontrado");}
            _mapper.Map(filmeDto, filme);
            await _context.SaveChangesAsync();
            if (_memoryCache.TryGetValue(FILMES_KEY, out List<Filme> filmes))
            {
               _memoryCache.Remove(FILMES_KEY);
               _memoryCache.Set(FILMES_KEY, filmes, memoryCachEntryOptions);
            }
            return Result.Ok();
        }
        public async Task<Result> DeletaFilme(int id)
        {
            Filme filme =  await _context.Filmes.FirstOrDefaultAsync(f => f.Id == id);
            if (filme == null) { return Result.Fail("Erro, Filme não encontrado"); }
            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();
            if (_memoryCache.TryGetValue(FILMES_KEY, out List<Filme> filmes))
            {
                filmes.Remove(filme);
                _memoryCache.Set(FILMES_KEY, filmes, memoryCachEntryOptions);
            }
            return Result.Ok();
        }
    }
}
