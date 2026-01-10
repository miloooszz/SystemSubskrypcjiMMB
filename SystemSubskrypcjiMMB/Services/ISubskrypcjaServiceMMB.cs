using SystemSubskrypcjiMMB.Models;

namespace SystemSubskrypcjiMMB.Services
{
    public interface ISubskrypcjaServiceMMB
    {
        Task<List<SubskrypcjaMMB>> PobierzUzytkownikaAsync(string userId);
        Task<SubskrypcjaMMB?> PobierzPoIdAsync(int id, string userId);
        Task<SubskrypcjaMMB> DodajAsync(SubskrypcjaMMB subskrypcja, string userId);
        Task<SubskrypcjaMMB?> AktualizujAsync(int id, SubskrypcjaMMB subskrypcja, string userId);
        Task<SubskrypcjaMMB?> UsunAsync(int id, string userId);
        Task<List<SubskrypcjaMMB>> PobierzZFiltramiAsync(string? nazwa, int? kategoriaId, string userId);
    }
}
