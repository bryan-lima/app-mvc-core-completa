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
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(fornecedor => fornecedor.Id);

            builder.Property(fornecedor => fornecedor.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(fornecedor => fornecedor.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Fornecedor : Endereco
            builder.HasOne(fornecedor => fornecedor.Endereco)
                .WithOne(endereco => endereco.Fornecedor);

            // 1 : N => Fornecedor : Produtos
            builder.HasMany(fornecedor => fornecedor.Produtos)
                .WithOne(produto => produto.Fornecedor)
                .HasForeignKey(produto => produto.FornecedorId);

            builder.ToTable("Fornecedores");
        }
    }
}
