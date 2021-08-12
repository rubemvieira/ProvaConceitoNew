using Microsoft.EntityFrameworkCore;
using ProvaConceito.DataAccess.Data;
using ProvaConceito.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaConceito.DataAccess.Services
{

    public class TurmaService
    {

        private readonly ProvaConceitoContext _provaConceitoContext;
        public TurmaService(ProvaConceitoContext provaConceitoContext) {
            _provaConceitoContext = provaConceitoContext;
        }

        public async Task<Turma> Create(Turma turma)
        {

            var escola = _provaConceitoContext.Escolas.Find(turma.EscolaId);
            escola.Turmas.Add(turma);
            await _provaConceitoContext.SaveChangesAsync();

            return turma;
        }

        public async Task<List<Turma>> GetAll()
        {
            List<Turma> turmas = await (_provaConceitoContext.Turmas.AsNoTracking()
                .OrderByDescending(o => o.Descricao)
                ).ToListAsync();

            return turmas;
        }

        public async Task<List<Turma>> GetAllWithChildren()
        {
            List<Turma> turmas = await (_provaConceitoContext.Turmas
                .Include(e => e.Escola)
                .Include(a => a.Alunos)
                .Include(p => p.Professores)
                .AsNoTracking()
                .OrderByDescending(o => o.Descricao)
                ).ToListAsync();

            return turmas;
        }

        private IQueryable<Turma> GetTurmaById(int? id) =>
            _provaConceitoContext.Turmas
                .Include(e => e.Escola)
                .Include(a => a.Alunos)
                .Include(p => p.Professores).ThenInclude(p => p.Professor)
                .AsNoTracking().Where(o => o.TurmaId == id);

        public async Task<Turma> GetById(int? id)
        {
            Turma turma = await GetTurmaById(id).FirstOrDefaultAsync();

            return turma;
        }
        public async Task<bool> Update(Turma turma)
        {
            bool isUpdated = false;
            Turma newturma = await GetTurmaById(turma.TurmaId).FirstOrDefaultAsync();

            if (newturma != null)
            {
                var escola = _provaConceitoContext.Escolas.Find(turma.EscolaId);
                escola.Turmas.Add(turma);
                _provaConceitoContext.Turmas.Update(turma);
                _provaConceitoContext.Escolas.Update(escola);
                await _provaConceitoContext.SaveChangesAsync();
                isUpdated = true;
            }

            return isUpdated;
        }

        public async Task<bool> Delete(int? id)
        {
            bool isDeleted = false;
            Turma turma = await GetTurmaById(id).FirstOrDefaultAsync();

            if (turma != null)
            {
                _provaConceitoContext.Remove(turma);
                await _provaConceitoContext.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}
