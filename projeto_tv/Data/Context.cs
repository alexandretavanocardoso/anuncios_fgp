using Microsoft.EntityFrameworkCore;
using projeto_tv.Models;

namespace vansyncenterprise.web.Repositorys.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}

        public DbSet<RegistroModel> Empresa { get; set; }
        public DbSet<AnuncioModel> Anuncio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
