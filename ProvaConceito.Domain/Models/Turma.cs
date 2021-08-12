using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProvaConceito.Domain.Models
{
    public class Turma
    {
        public int TurmaId { get; set; }
        public string Descricao { get; set; }
        public string Turno { get; set; }
        public string Serie { get; set; }
        public int EscolaId { get; set; }

        [ForeignKey("EscolaId")]
        public Escola Escola { get; set; }
        public ICollection<Aluno> Alunos { get; } = new List<Aluno>();
        public ICollection<ProfessorTurma> Professores { get; set; }

    }
}
