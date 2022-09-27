namespace AutenticacaoUsuario.API.Dtos
{
    public class DtoRefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Criado { get; set; } = DateTime.Now;
        public DateTime Expira { get; set; }
    }
}
