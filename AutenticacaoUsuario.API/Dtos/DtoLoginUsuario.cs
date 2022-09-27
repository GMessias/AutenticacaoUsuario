using System.ComponentModel.DataAnnotations;

namespace AutenticacaoUsuario.API.Dtos
{
    public class DtoLoginUsuario
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}
