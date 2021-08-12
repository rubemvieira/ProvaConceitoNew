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

    public class EscolaService
    {

        private readonly ProvaConceitoContext _provaConceitoContext;
        public EscolaService(ProvaConceitoContext provaConceitoContext) {
            _provaConceitoContext = provaConceitoContext;
        }

        public async Task<Escola> Create(Escola escola)
        {
            _provaConceitoContext.Escolas.Add(escola);
            await _provaConceitoContext.SaveChangesAsync();

            return escola;
        }

        public async Task<List<Escola>> GetAll()
        {
            List<Escola> escolas = await (_provaConceitoContext.Escolas.AsNoTracking()
                .OrderByDescending(o => o.CodigoINEP)
                ).ToListAsync();

            return escolas;
        }

        private IQueryable<Escola> GetEscolaById(int? id) =>
            _provaConceitoContext.Escolas
                .Include(e => e.Turmas)
                .ThenInclude(t => t.Alunos)
                .Include(e => e.Turmas)
                .ThenInclude(t => t.Professores)
                .ThenInclude(t => t.Professor)
                .AsNoTracking().Where(o => o.EscolaId == id);


        public async Task<Escola> GetById(int? id)
        {
            Escola escola = await GetEscolaById(id).FirstOrDefaultAsync();

            return escola;
        }
        public async Task<bool> Update(Escola escola)
        {
            bool isUpdated = false;
            Escola newescola = await GetEscolaById(escola.EscolaId).FirstOrDefaultAsync();

            if (newescola != null)
            {
                _provaConceitoContext.Escolas.Update(escola);
                await _provaConceitoContext.SaveChangesAsync();
                isUpdated = true;
            }

            return isUpdated;
        }

        public async Task<bool> Delete(int? id)
        {
            bool isDeleted = false;
            Escola escola = await GetEscolaById(id).FirstOrDefaultAsync();

            if (escola != null)
            {
                _provaConceitoContext.Remove(escola);
                await _provaConceitoContext.SaveChangesAsync();
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}
