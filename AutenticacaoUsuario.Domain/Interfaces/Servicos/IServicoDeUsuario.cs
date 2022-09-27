using AutenticacaoUsuario.Domain.Models;

namespace AutenticacaoUsuario.Domain.Interfaces.Servicos
{
    public interface IServicoDeUsuario
    {
        bool VerificarExistenciaDeUsuario(string email);
        Usuario RegistrarUsuarioComSenhaGerada(string nome, string email, string senha);
        void AdicionarUsuarioNoBancoDeDados(Usuario usuario);
        Usuario ConsultarUsuarioPorEmail(string email);
        void AtribuirRefreshTokenNoUsuario(RefreshToken refreshToken, Usuario usuario);
        void SalvarModificacoes(Usuario usuario);
        Usuario ConsultarUsuarioPorToken(string token);
        void VerificarTokenDoUsuario(Usuario usuario);
        Usuario ConsultarUsuarioUtilizandoResetToken(string resetToken);
        void FazerResetDaSenha(Usuario usuario, string resetSenha);
    }
}
