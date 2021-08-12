using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaConceito.FrontEnd.Models
{
    public class ProfessorViewModel
    {
        [DisplayName("Código")] 
        public int ProfessorId { get; set; }
        public string Nome { get; set; }
        public string Materia { get; set; }
        [DisplayName("Turmas")] 
        public ICollection<ProfessorTurmaViewModel> Turmas { get; set; }
    }
}
