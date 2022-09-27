using AutenticacaoUsuario.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace AutenticacaoUsuario.Domain.Interfaces.Servicos
{
    public interface IServicoDeToken
    {
        string CriarTokenAleatorio();
        string CriarToken(Usuario usuario, IConfiguration _configuration);
        RefreshToken GerarRefreshToken();
    }
}
