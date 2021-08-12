using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaConceito.FrontEnd.Models
{
    public class TurmaViewModel
    {
        [DisplayName("Código")]
        public int TurmaId { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Série")]
        public string Serie { get; set; }
        public string Turno { get; set; }

        [DisplayName("Escola")]
        public int EscolaId { get; set; }
        public EscolaViewModel Escola { get; set; }
        public ICollection<AlunoViewModel> Alunos { get; } = new List<AlunoViewModel>();

        [DisplayName("Professores")]
        public ICollection<ProfessorTurmaViewModel> Professores { get; set; }
    }
}
