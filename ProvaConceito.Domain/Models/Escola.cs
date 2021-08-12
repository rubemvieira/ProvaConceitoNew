using System;
using System.Collections.Generic;
using System.Text;

namespace ProvaConceito.Domain.Models
{
    public class Escola
    {
        public int EscolaId { get; set; }
        public string Nome { get; set; }
        public string CodigoINEP { get; set; }
        public ICollection<Turma> Turmas { get; } = new List<Turma>();
    }
}
