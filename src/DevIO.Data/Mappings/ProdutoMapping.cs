using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(produto => produto.Id);

            builder.Property(produto => produto.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(produto => produto.Descricao)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(produto => produto.Imagem)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.ToTable("Produtos");
        }
    }
}
