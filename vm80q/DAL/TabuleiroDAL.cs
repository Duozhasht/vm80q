using vm80q.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace vm80q.DAL
{
    public class TabuleiroDAL : DbContext
    {

        public TabuleiroDAL() : base("basedadosContext") { }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }


        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}