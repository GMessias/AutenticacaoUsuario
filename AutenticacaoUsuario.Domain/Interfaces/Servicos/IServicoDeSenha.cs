namespace AutenticacaoUsuario.Domain.Interfaces.Servicos
{
    public interface IServicoDeSenha
    {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
    }
}
