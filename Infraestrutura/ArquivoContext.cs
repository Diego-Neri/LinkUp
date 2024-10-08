using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Infraestrutura {
    public class ArquivoContext : DbContext {
        public ArquivoContext(DbContextOptions<ArquivoContext> options) : base(options) { 
        
        }

        public DbSet<Arquivos> Arquivos { get; set; }

    }
}
