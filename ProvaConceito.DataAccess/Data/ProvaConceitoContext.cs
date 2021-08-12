using Microsoft.EntityFrameworkCore;
using System;
using ProvaConceito.Domain.Models;

namespace ProvaConceito.DataAccess.Data
{
    public partial class ProvaConceitoContext : DbContext
    {

        // Modelos
        public DbSet<Aluno> Alunos { set; get; }
        public DbSet<Escola> Escolas { set; get; }
        public DbSet<Professor> Professores { set; get; }
        public DbSet<Turma> Turmas { set; get; }
        public DbSet<ProfessorTurma> ProfessoresTurmas { set; get; }
        
        public string DbPath { get; private set; }
        public ProvaConceitoContext(DbContextOptions<ProvaConceitoContext> options) : base(options)
        {
            Database.Migrate();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Turma>()
                .HasOne(e => e.Escola)
                .WithMany(c => c.Turmas);

            modelBuilder.Entity<Aluno>()
                .HasOne(e => e.Turma)
                .WithMany(c => c.Alunos);

            modelBuilder.Entity<ProfessorTurma>()
                    .HasKey(pt => new { pt.ProfessorId, pt.TurmaId });

            modelBuilder.Entity<ProfessorTurma>()
                .HasOne(pt => pt.Professor)
                .WithMany(b => b.Turmas)
                .HasForeignKey(pt => pt.ProfessorId);

            modelBuilder.Entity<ProfessorTurma>()
                .HasOne(pt => pt.Turma)
                .WithMany(c => c.Professores)
                .HasForeignKey(pt => pt.TurmaId);
        }

    }
}
