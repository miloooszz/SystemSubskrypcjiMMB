using System.Globalization;
using SystemSubskrypcjiMMB.Models;
using SystemSubskrypcjiMMB.Repositories;
using SystemSubskrypcjiMMB.Services;

namespace SystemSubskrypcjiMMB.Services
{
    public class SubskrypcjaServiceMMB : ISubskrypcjaServiceMMB
    {
        private readonly ISubskrypcjaRepositoryMMB _repository;

        public SubskrypcjaServiceMMB(ISubskrypcjaRepositoryMMB repository)
        {
            _repository = repository;
        }

        public async Task<List<SubskrypcjaMMB>> PobierzUzytkownikaAsync(string userId)
        {
            return await _repository.PobierzUzytkownikaAsync(userId);
        }

        public async Task<SubskrypcjaMMB?> PobierzPoIdAsync(int id, string userId)
        {
            var subskrypcja = await _repository.PobierzPoIdAsync(id);
            if (subskrypcja?.UzytkownikId != userId)
                return null;
            return subskrypcja;
        }

        public async Task<SubskrypcjaMMB> DodajAsync(SubskrypcjaMMB subskrypcja, string userId)
        {
            subskrypcja.UzytkownikId = userId;
            return await _repository.DodajAsync(subskrypcja);
        }

        public async Task<SubskrypcjaMMB?> AktualizujAsync(int id, SubskrypcjaMMB subskrypcja, string userId)
        {
            var existing = await PobierzPoIdAsync(id, userId);
            if (existing == null) return null;
            return await _repository.AktualizujAsync(id, subskrypcja);
        }

        public async Task<SubskrypcjaMMB?> UsunAsync(int id, string userId)
        {
            var existing = await PobierzPoIdAsync(id, userId);
            if (existing == null) return null;
            return await _repository.UsunAsync(id);
        }

        public async Task<List<SubskrypcjaMMB>> PobierzZFiltramiAsync(string? nazwa, int? kategoriaId, string userId)
        {
            var wszystkie = await _repository.PobierzZFiltramiAsync(nazwa, kategoriaId);
            return wszystkie.Where(s => s.UzytkownikId == userId).ToList();
        }

        // NOWE: STATYSTYKI ORM ADVANCED
        public async Task<object> PobierzStatystykiAsync(string userId)
        {
            var subskrypcje = await PobierzUzytkownikaAsync(userId);

            var miesieczne = subskrypcje
                .GroupBy(s => s.DataRozpoczecia.Month)
                .Select(g => new
                {
                    Miesiac = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Suma = g.Sum(s => (double)s.Cena),
                    Liczba = g.Count(),
                    Przecietnia = Math.Round(g.Average(s => (double)s.Cena), 2)
                })
                .OrderByDescending(g => g.Suma)
                .ToList();

            return new
            {
                LiczbaSubskrypcji = subskrypcje.Count,
                SumaWydatkow = subskrypcje.Sum(s => s.Cena),
                Najdrozsza = subskrypcje.MaxBy(s => s.Cena)?.Nazwa ?? "Brak",
                PrzecietniaCena = Math.Round(subskrypcje.Average(s => (double)s.Cena), 2),
                MiesieczneWydatki = miesieczne
            };
        }
    }
}
