using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adopcja_2024.Models
{
    public class Miasto
    {
        [Key]
        public int id_miasto { get; set; }

        [Display(Name = "Nazwa miasta")]
        [Required(ErrorMessage = "Miasto jest wymagane!")]
        [DataType(DataType.Text)]
        public string nazwa { get; set; }

        public ICollection<Osoba>? Osoby { get; set; }
        public ICollection<Instytucja>? Instytucje { get; set; }

        public Miasto()
        {

        }
    }
}
