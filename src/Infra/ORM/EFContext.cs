using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Domain.VOs;
using Domain.Entidades;
namespace Infra;

public class EFContext : DbContext
{

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.ToTable("Cliente");
            entity.HasKey(e => e.Id);

            entity.HasMany(c => c.Veiculos)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.ClienteId);

            entity.OwnsOne(c => c.Documento, documento =>
            {
                documento.Property(d => d.Numero)
                .HasColumnName("Documento")
                .HasMaxLength(14)
                .IsRequired();

                documento.Property(d => d.Tipo)
                .HasColumnName("TipoDocumento")
                .HasMaxLength(4)
                .IsRequired();

                documento.HasIndex(d => d.Numero).IsUnique();
            });

            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            
            entity.OwnsOne(c => c.Email, email =>
            {
                email.Property(e => e.Endereco)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsRequired();

                email.HasIndex(e => e.Endereco).IsUnique();
            });

            entity.OwnsOne(c => c.Celular, celular =>
            {
                celular.Property(c => c.Numero)
                    .HasColumnName("Celular")
                    .HasMaxLength(11)
                    .IsRequired();

                celular.HasIndex(c => c.Numero).IsUnique();
            });
        });

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.ToTable("Veiculo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.Modelo).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Marca).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Ano).IsRequired();
            entity.HasOne(v => v.Cliente)
                .WithMany(c => c.Veiculos)
                .HasForeignKey(v => v.ClienteId);

                entity.OwnsOne(v => v.Placa, placa =>
                {
                    placa.Property(p => p.Valor)
                        .HasColumnName("Placa")
                        .HasMaxLength(8)
                        .IsRequired();

                    placa.HasIndex(p => p.Valor).IsUnique();
                });
        });
    }
}
