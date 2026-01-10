using System.ComponentModel.DataAnnotations;

namespace SystemSubskrypcjiMMB.Models
{
    public class KategoriaMMB
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa kategorii jest wymagana")]
        [StringLength(50, ErrorMessage = "Nazwa nie może przekraczać 50 znaków")]
        public string Nazwa { get; set; } = string.Empty;

        public virtual ICollection<SubskrypcjaMMB> Subskrypcje { get; set; } = new List<SubskrypcjaMMB>();
    }
}
