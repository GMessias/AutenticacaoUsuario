using AutenticacaoUsuario.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AutenticacaoUsuario.Infrastructure.Data
{
    public class UsuarioDataContext : DbContext
    {
        public UsuarioDataContext(DbContextOptions<UsuarioDataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer("Server=.\\SQLEXPRESS;Database=usuariodb;Trusted_Connection=true;");
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
    }
}
