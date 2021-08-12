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

    public class AlunoService
    {

        private readonly ProvaConceitoContext _provaConceitoContext;
        public AlunoService(ProvaConceitoContext provaConceitoContext) {
            _provaConceitoContext = provaConceitoContext;
        }

        public async Task<Aluno> Create(Aluno aluno)
        {
            var turma = _provaConceitoContext.Turmas.Find(aluno.TurmaId);
            turma.Alunos.Add(aluno);
            await _provaConceitoContext.SaveChangesAsync();

            return aluno;
        }

        public async Task<List<Aluno>> GetAll()
        {
            List<Aluno> alunos = await (_provaConceitoContext.Alunos.AsNoTracking()
                .OrderByDescending(o => o.Nome)
                ).ToListAsync();

            return alunos;
        }

        private IQueryable<Aluno> GetAlunoById(int? id) =>
            _provaConceitoContext.Alunos
                .Include(t => t.Turma)
                .ThenInclude(t => t.Escola)
                .AsNoTracking().Where(o => o.AlunoId == id);

        public async Task<Aluno> GetById(int? id)
        {
            Aluno aluno = await GetAlunoById(id).FirstOrDefaultAsync();

            return aluno;
        }
        public async Task<bool> Update(Aluno aluno)
        {
            bool isUpdated = false;
            Aluno newaluno = await GetAlunoById(aluno.AlunoId).FirstOrDefaultAsync();

            if (newaluno != null)
            {
                var turma = _provaConceitoContext.Turmas.Find(aluno.TurmaId);
                turma.Alunos.Add(aluno);
                _provaConceitoContext.Alunos.Update(aluno);
                _provaConceitoContext.Turmas.Update(turma);
                await _provaConceitoContext.SaveChangesAsync();
                isUpdated = true;
            }

            return isUpdated;
        }

        public async Task<bool> Delete(int? id)
        {
            bool isDeleted = false;
            Aluno aluno = await GetAlunoById(id).FirstOrDefaultAsync();

            if (aluno != null)
            {
                _provaConceitoContext.Remove(aluno);
                await _provaConceitoContext.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}
