using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SystemSubskrypcjiMMB.Models;
using SystemSubskrypcjiMMB.Services;

namespace SystemSubskrypcjiMMB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminKontrolerMMB : Controller
    {
        private readonly ISubskrypcjaServiceMMB _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminKontrolerMMB(ISubskrypcjaServiceMMB service,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _service = service;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Statystyki()
        {
            var wszystkie = await _service.PobierzUzytkownikaAsync("wszystkie");
            // Grupowanie, sumowanie - ORM advanced
            var stats = wszystkie.GroupBy(s => s.DataRozpoczecia.Month)
                .Select(g => new { Miesiac = g.Key, Suma = g.Sum(s => s.Cena), Liczba = g.Count() });

            ViewBag.Stats = stats;
            return View();
        }
    }
}
