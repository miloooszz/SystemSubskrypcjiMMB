using SystemSubskrypcjiMMB.Models;

namespace SystemSubskrypcjiMMB.Repositories
{
    public interface ISubskrypcjaRepositoryMMB
    {
        Task<List<SubskrypcjaMMB>> PobierzWszystkieAsync();
        Task<List<SubskrypcjaMMB>> PobierzUzytkownikaAsync(string userId);
        Task<SubskrypcjaMMB?> PobierzPoIdAsync(int id);
        Task<SubskrypcjaMMB> DodajAsync(SubskrypcjaMMB subskrypcja);
        Task<SubskrypcjaMMB?> AktualizujAsync(int id, SubskrypcjaMMB subskrypcja);
        Task<SubskrypcjaMMB?> UsunAsync(int id);
        Task<List<SubskrypcjaMMB>> PobierzZFiltramiAsync(string? nazwa, int? kategoriaId);
    }
}
