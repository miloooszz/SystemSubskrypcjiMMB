using System.ComponentModel.DataAnnotations;

namespace SystemSubskrypcjiMMB.Models
{
    public class SubskrypcjaMMB
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Nazwa 2-100 znaków")]
        public string Nazwa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Range(0.01, 1000, ErrorMessage = "Cena 0.01-1000zł")]
        public decimal Cena { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia wymagana")]
        public DateTime DataRozpoczecia { get; set; }

        public DateTime? DataZakonczenia { get; set; }

        public string UzytkownikId { get; set; } = string.Empty;
        public virtual ApplicationUser? Uzytkownik { get; set; }
        public int? KategoriaId { get; set; }
        public virtual KategoriaMMB? Kategoria { get; set; }
    }
}
