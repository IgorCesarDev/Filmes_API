using FilmesFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FilmesFinal.Services
{
    public class TokenService
    {
        public Token CreateToken(CustomIdentityUser usuario, string role)
        {
            Claim[] direitosUsuario = new Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0asdjas09djsa09djasdjsadajsd09asjd09sajcnzxn"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: direitosUsuario,
                signingCredentials: credenciais,
                expires: DateTime.UtcNow.AddHours(1));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new Token(tokenString);
        }
    }
}
