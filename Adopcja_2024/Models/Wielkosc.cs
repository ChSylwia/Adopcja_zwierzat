using System.ComponentModel.DataAnnotations;

namespace Adopcja_2024.Models
{
    public class Wielkosc
    {
        [Key]
        public int id_wielkosc { get; set; }
        public string wielkosc { get; set; }
        public ICollection<Zwierzak> Zwierzak { get; set; }
        public Wielkosc() { }
    }
}
