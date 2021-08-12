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

    public class ProfessorService
    {

        private readonly ProvaConceitoContext _provaConceitoContext;
        public ProfessorService(ProvaConceitoContext provaConceitoContext) {
            _provaConceitoContext = provaConceitoContext;
        }

        public async Task<Professor> Create(Professor professor)
        {

            _provaConceitoContext.Professores.Add(professor);
            await _provaConceitoContext.SaveChangesAsync();

            return professor;
        }

        public async Task<List<Professor>> GetAll()
        {
            List<Professor> professors = await (_provaConceitoContext.Professores.AsNoTracking()
                .OrderByDescending(o => o.Materia)
                .ThenByDescending(o => o.Nome)
                ).ToListAsync();

            return professors;
        }

        private IQueryable<Professor> GetProfessorById(int? id) =>
            _provaConceitoContext.Professores
                .Include(p => p.Turmas)
                .ThenInclude(p => p.Turma)
                .ThenInclude(p => p.Escola)
                .AsNoTracking().Where(o => o.ProfessorId == id);

        public async Task<Professor> GetById(int? id)
        {
            Professor professor = await GetProfessorById(id).FirstOrDefaultAsync();

            return professor;
        }
        public async Task<bool> Update(Professor professor)
        {
            bool isUpdated = false;
            Professor newprofessor = await GetProfessorById(professor.ProfessorId).FirstOrDefaultAsync();

            if (newprofessor != null)
            {
                // Vamos excluir a referencia turma/professor
                foreach (var turma in newprofessor.Turmas)
                {
                    var professorturmaCheck = _provaConceitoContext.ProfessoresTurmas.Find(professor.ProfessorId, turma.TurmaId);
                    _provaConceitoContext.ProfessoresTurmas.Remove(professorturmaCheck);
                }
                newprofessor.Turmas = new List<ProfessorTurma>();
                _provaConceitoContext.Professores.Update(newprofessor);
                await _provaConceitoContext.SaveChangesAsync();

                foreach (var turma in professor.Turmas)
                {
                    var newturma = _provaConceitoContext.Turmas.Include(p => p.Professores).Where(t => t.TurmaId == turma.TurmaId).Single();
                    if (newturma.Professores == null) newturma.Professores = new List<ProfessorTurma>();
                    ProfessorTurma professorturma = new ProfessorTurma
                    {
                        ProfessorId = professor.ProfessorId,
                        TurmaId = turma.TurmaId,
                        Professor = newprofessor,
                        Turma = newturma
                    };


                    _provaConceitoContext.ProfessoresTurmas.Add(professorturma);
                    if(!newprofessor.Turmas.Contains(professorturma))
                        newprofessor.Turmas.Add(professorturma);
                    if(!newturma.Professores.Contains(professorturma))
                        newturma.Professores.Add(professorturma);

                    _provaConceitoContext.Turmas.Update(newturma);
                }

                _provaConceitoContext.Professores.Update(newprofessor);
                await _provaConceitoContext.SaveChangesAsync();
                isUpdated = true;
            }

            return isUpdated;
        }

        public async Task<bool> Delete(int? id)
        {
            bool isDeleted = false;
            Professor professor = await GetProfessorById(id).FirstOrDefaultAsync();

            if (professor != null)
            {
                _provaConceitoContext.Remove(professor);
                await _provaConceitoContext.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}
