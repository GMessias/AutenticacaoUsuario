using AutenticacaoUsuario.Domain.Models;

namespace AutenticacaoUsuario.Domain.Interfaces.Repositorios
{
    public interface IRepositorioDeUsuario
    {
        bool UsuarioCadastrado(string email);
        void CadastrarUsuario(Usuario usuario);
        Usuario ConsultarUsuarioComEmail(string email);
        void AtualizarModificacoes(Usuario usuario);
        Usuario ConsultarUsuarioPorToken(string token);
        Usuario ConsultarUsuarioPorResetToken(string resetToken);
    }
}
