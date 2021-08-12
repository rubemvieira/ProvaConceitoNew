using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaConceito.FrontEnd.Models
{
    public class EscolaViewModel
    {
        [DisplayName("Código")]
        public int EscolaId { get; set; }
        public string Nome { get; set; }

        [DisplayName("Código INEP")]
        public string CodigoINEP { get; set; }
        public ICollection<TurmaViewModel> Turmas { get; } = new List<TurmaViewModel>();
    }
}
