using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adopcja_2024.Models
{
    public class Rezerwacja
    {
        [Key]
        public int id_rezerwacja { get; set; }
        [ForeignKey("Zwierzak")]
        public int id_zwierzak { get; set; }
        [ForeignKey("Osoba")]
        public int id_osoba { get; set; }
        public Osoba Osoba { get; set; }
        public Zwierzak Zwierzak { get; set; }
        public Rezerwacja() { }
    }
}
