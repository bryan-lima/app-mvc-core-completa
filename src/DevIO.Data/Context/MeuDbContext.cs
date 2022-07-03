using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DevIO.Data.Context
{
    public class MeuDbContext : DbContext
    {
        public MeuDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableProperty property in modelBuilder.Model.GetEntityTypes()
                                                                                                           .SelectMany(entity => entity.GetProperties()
                                                                                                                                       .Where(property => property.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

            foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes()
                                                                                                                 .SelectMany(entity => entity.GetForeignKeys())) 
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
