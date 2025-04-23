using System.ComponentModel.DataAnnotations;

namespace Adopcja_2024.Models
{
    public class Wiek
    {
        [Key]
        public int id_wiek { get; set; }
        public string wiek { get; set; }
        public ICollection<Zwierzak> Zwierzak { get; set; }
        public Wiek() { }
    }
}
