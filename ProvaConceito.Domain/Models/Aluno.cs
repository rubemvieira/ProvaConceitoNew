using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProvaConceito.Domain.Models
{
    public class Aluno
    {
        public int AlunoId { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public int TurmaId { get; set; }

        [ForeignKey("TurmaId")]
        public Turma Turma { get; set; }
    }
}
