using Microsoft.EntityFrameworkCore;
using RecrutamentoDiversidade.Models;

namespace RecrutamentoDiversidade.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Vaga> Vagas { get; set; }
    public DbSet<Candidato> Candidatos { get; set; }
    public DbSet<Candidatura> Candidaturas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("TB_USUARIO_NET");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                  .HasColumnType("NUMBER(19)")
                  .HasDefaultValueSql("SEQ_USUARIO.NEXTVAL")
                  .ValueGeneratedOnAdd();

            entity.Property(u => u.Nome)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(150)");

            entity.Property(u => u.Email)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(255)");

            entity.HasIndex(u => u.Email)
                  .IsUnique()
                  .HasDatabaseName("UK_USUARIO_NET_EMAIL");

            entity.Property(u => u.Senha)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(255)");

            entity.Property(u => u.Role)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(20)")
                  .HasConversion<string>();

            entity.Property(u => u.Ativo)
                  .IsRequired()
                  .HasColumnType("NUMBER(1)")
                  .HasDefaultValue(1);

            entity.Property(u => u.CriadoEm)
                  .IsRequired()
                  .HasColumnType("TIMESTAMP")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Vaga>(entity =>
        {
            entity.ToTable("TB_VAGA_NET");

            entity.HasKey(v => v.Id);

            entity.Property(v => v.Id)
                  .HasColumnType("NUMBER(19)")
                  .HasDefaultValueSql("SEQ_VAGA.NEXTVAL")
                  .ValueGeneratedOnAdd();

            entity.Property(v => v.Titulo)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(200)");

            entity.Property(v => v.Descricao)
                  .HasColumnType("NCLOB");

            entity.Property(v => v.Departamento)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(100)");

            entity.Property(v => v.LocalTrabalho)
                  .HasColumnType("NVARCHAR2(150)");

            entity.Property(v => v.TipoContrato)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(30)")
                  .HasConversion<string>();

            entity.Property(v => v.Status)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(20)")
                  .HasConversion<string>()
                  .HasDefaultValue(StatusVaga.Aberta);

            entity.Property(v => v.MetaDiversidadePct)
                  .IsRequired()
                  .HasColumnType("NUMBER(5,2)")
                  .HasDefaultValue(30m);

            entity.Property(v => v.CriadoEm)
                  .IsRequired()
                  .HasColumnType("TIMESTAMP")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(v => v.EncerradoEm)
                  .HasColumnType("TIMESTAMP");

            entity.HasOne(v => v.Usuario)
                  .WithMany(u => u.Vagas)
                  .HasForeignKey(v => v.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_VAGA_NET_USUARIO");
        });

        modelBuilder.Entity<Candidato>(entity =>
        {
            entity.ToTable("TB_CANDIDATO_NET");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id)
                  .HasColumnType("NUMBER(19)")
                  .HasDefaultValueSql("SEQ_CANDIDATO.NEXTVAL")
                  .ValueGeneratedOnAdd();

            entity.Property(c => c.Nome)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(150)");

            entity.Property(c => c.Email)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(255)");

            entity.HasIndex(c => c.Email)
                  .IsUnique()
                  .HasDatabaseName("UK_CANDIDATO_NET_EMAIL");

            entity.Property(c => c.Telefone)
                  .HasColumnType("NVARCHAR2(20)");

            entity.Property(c => c.Linkedin)
                  .HasColumnType("NVARCHAR2(255)");

            entity.Property(c => c.CurriculoUrl)
                  .HasColumnType("NVARCHAR2(500)");

            entity.Property(c => c.Genero)
                  .HasColumnType("NVARCHAR2(30)")
                  .HasConversion<string>();

            entity.Property(c => c.RacaEtnia)
                  .HasColumnType("NVARCHAR2(30)")
                  .HasConversion<string>();

            entity.Property(c => c.PessoaComDeficiencia)
                  .IsRequired()
                  .HasColumnType("NUMBER(1)")
                  .HasDefaultValue(0);

            entity.Property(c => c.TipoDeficiencia)
                  .HasColumnType("NVARCHAR2(100)");

            entity.Property(c => c.CriadoEm)
                  .IsRequired()
                  .HasColumnType("TIMESTAMP")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Candidatura>(entity =>
        {
            entity.ToTable("TB_CANDIDATURA_NET");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Id)
                  .HasColumnType("NUMBER(19)")
                  .HasDefaultValueSql("SEQ_CANDIDATURA.NEXTVAL")
                  .ValueGeneratedOnAdd();

            entity.Property(c => c.Status)
                  .IsRequired()
                  .HasColumnType("NVARCHAR2(30)")
                  .HasConversion<string>()
                  .HasDefaultValue(StatusCandidatura.Inscrito);

            entity.Property(c => c.PrioridadeDiversidade)
                  .IsRequired()
                  .HasColumnType("NUMBER(1)")
                  .HasDefaultValue(0);

            entity.Property(c => c.Observacoes)
                  .HasColumnType("NVARCHAR2(1000)");

            entity.Property(c => c.InscritoEm)
                  .IsRequired()
                  .HasColumnType("TIMESTAMP")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(c => c.AtualizadoEm)
                  .IsRequired()
                  .HasColumnType("TIMESTAMP")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(c => new { c.CandidatoId, c.VagaId })
                  .IsUnique()
                  .HasDatabaseName("UK_CANDIDATURA_NET_UNICA");

            entity.HasOne(c => c.Candidato)
                  .WithMany(ca => ca.Candidaturas)
                  .HasForeignKey(c => c.CandidatoId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_CANDIDATURA_NET_CANDIDATO");

            entity.HasOne(c => c.Vaga)
                  .WithMany(v => v.Candidaturas)
                  .HasForeignKey(c => c.VagaId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_CANDIDATURA_NET_VAGA");
        });
    }
}