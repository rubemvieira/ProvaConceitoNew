using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaConceito.FrontEnd.Models
{
    public class AlunoViewModel
    {
        [DisplayName("Código")] 
        public int AlunoId { get; set; }
        public string Nome { get; set; }

        [DisplayName("Data de nascimento")] 
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }

        [DisplayName("Turma")]
        public int TurmaId { get; set; }
        public TurmaViewModel Turma { get; set; }
    }
}
