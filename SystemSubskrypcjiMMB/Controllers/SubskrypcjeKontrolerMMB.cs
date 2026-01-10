using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SystemSubskrypcjiMMB.Models;
using SystemSubskrypcjiMMB.Services;

namespace SystemSubskrypcjiMMB.Controllers
{
    [Authorize]
    public class SubskrypcjeKontrolerMMB : Controller
    {
        private readonly ISubskrypcjaServiceMMB _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubskrypcjeKontrolerMMB(ISubskrypcjaServiceMMB service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search = null, int? kategoriaId = null)
        {
            var userId = _userManager.GetUserId(User)!;
            var subskrypcje = await _service.PobierzZFiltramiAsync(search, kategoriaId, userId);

            ViewBag.Search = search;
            ViewBag.KategoriaId = kategoriaId ?? 0;
            return View(subskrypcje);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create (POPRAWIONE - bez nadpisywania daty!)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubskrypcjaMMB subskrypcja)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User)!;
                subskrypcja.UzytkownikId = userId;
                // DataRozpoczecia = z formularza użytkownika!

                await _service.DodajAsync(subskrypcja, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(subskrypcja);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var subskrypcja = await _service.PobierzPoIdAsync(id.Value, userId);
            if (subskrypcja == null) return NotFound();

            return View(subskrypcja);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubskrypcjaMMB subskrypcja)
        {
            if (id != subskrypcja.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User)!;
                await _service.AktualizujAsync(id, subskrypcja, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(subskrypcja);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var subskrypcja = await _service.PobierzPoIdAsync(id.Value, userId);
            if (subskrypcja == null) return NotFound();

            return View(subskrypcja);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            await _service.UsunAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DaneTestowe()
        {
            var userId = _userManager.GetUserId(User)!;

            var testowe = new[]
            {
                new SubskrypcjaMMB { Nazwa = "Netflix", Cena = 49.99m, DataRozpoczecia = DateTime.Today.AddDays(-30) },
                new SubskrypcjaMMB { Nazwa = "Spotify", Cena = 19.99m, DataRozpoczecia = DateTime.Today.AddDays(-60) },
                new SubskrypcjaMMB { Nazwa = "Disney+", Cena = 29.99m, DataRozpoczecia = DateTime.Today.AddDays(-10) }
            };

            foreach (var s in testowe)
            {
                s.UzytkownikId = userId;
                await _service.DodajAsync(s, userId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
