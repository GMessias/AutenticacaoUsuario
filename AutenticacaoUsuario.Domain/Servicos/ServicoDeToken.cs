using AutenticacaoUsuario.Domain.Interfaces.Servicos;
using AutenticacaoUsuario.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AutenticacaoUsuario.Domain.Servicos
{
    public class ServicoDeToken : IServicoDeToken
    {
        public string CriarTokenAleatorio()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public string CriarToken(Usuario usuario, IConfiguration _configuration)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public RefreshToken GerarRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expira = DateTime.Now.AddDays(7),
                Criado = DateTime.Now
            };

            return refreshToken;
        }
    }
}
