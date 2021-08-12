using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaConceito.FrontEnd.Models
{
    public class ProfessorTurmaViewModel
    {
        public int ProfessorId { get; set; }
        public ProfessorViewModel Professor { get; set; }
        public int TurmaId { get; set; }
        public TurmaViewModel Turma { get; set; }
    }
}
