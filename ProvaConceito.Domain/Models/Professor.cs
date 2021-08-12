using System;
using System.Collections.Generic;
using System.Text;

namespace ProvaConceito.Domain.Models
{
    public class Professor
    {
        public int ProfessorId { get; set; }
        public string Nome { get; set; }
        public string Materia { get; set; }
        public ICollection<ProfessorTurma> Turmas { get; set; } 

    }
}
