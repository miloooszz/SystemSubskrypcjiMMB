using Microsoft.EntityFrameworkCore;
using SystemSubskrypcjiMMB.Data;
using SystemSubskrypcjiMMB.Models;
using SystemSubskrypcjiMMB.Repositories;

namespace SystemSubskrypcjiMMB.Repositories
{
    public class SubskrypcjaRepositoryMMB : ISubskrypcjaRepositoryMMB
    {
        private readonly DbContextMMB _context;

        public SubskrypcjaRepositoryMMB(DbContextMMB context)
        {
            _context = context;
        }

        public async Task<List<SubskrypcjaMMB>> PobierzWszystkieAsync()
        {
            return await _context.Subskrypcje
                .Include(s => s.Uzytkownik)
                .Include(s => s.Kategoria)
                .ToListAsync();
        }

        public async Task<List<SubskrypcjaMMB>> PobierzUzytkownikaAsync(string userId)
        {
            return await _context.Subskrypcje
                .Where(s => s.UzytkownikId == userId)
                .Include(s => s.Kategoria)
                .ToListAsync();
        }

        public async Task<SubskrypcjaMMB?> PobierzPoIdAsync(int id)
        {
            return await _context.Subskrypcje
                .Include(s => s.Kategoria)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SubskrypcjaMMB> DodajAsync(SubskrypcjaMMB subskrypcja)
        {
            _context.Subskrypcje.Add(subskrypcja);
            await _context.SaveChangesAsync();
            return subskrypcja;
        }

        public async Task<SubskrypcjaMMB?> AktualizujAsync(int id, SubskrypcjaMMB subskrypcja)
        {
            var existing = await _context.Subskrypcje.FindAsync(id);
            if (existing == null) return null;

            existing.Nazwa = subskrypcja.Nazwa;
            existing.Cena = subskrypcja.Cena;
            existing.DataRozpoczecia = subskrypcja.DataRozpoczecia;
            existing.DataZakonczenia = subskrypcja.DataZakonczenia;
            existing.KategoriaId = subskrypcja.KategoriaId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<SubskrypcjaMMB?> UsunAsync(int id)
        {
            var subskrypcja = await _context.Subskrypcje.FindAsync(id);
            if (subskrypcja == null) return null;

            _context.Subskrypcje.Remove(subskrypcja);
            await _context.SaveChangesAsync();
            return subskrypcja;
        }

        public async Task<List<SubskrypcjaMMB>> PobierzZFiltramiAsync(string? nazwa, int? kategoriaId)
        {
            var query = _context.Subskrypcje.Include(s => s.Kategoria).AsQueryable();

            if (!string.IsNullOrEmpty(nazwa))
                query = query.Where(s => s.Nazwa.Contains(nazwa));

            if (kategoriaId.HasValue)
                query = query.Where(s => s.KategoriaId == kategoriaId);

            return await query.ToListAsync();
        }
    }
}
