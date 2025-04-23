using System.ComponentModel.DataAnnotations;

namespace Adopcja_2024.Models
{
    public class Gatunek
    {
        [Key]
        public int id_gatunek { get; set; }
        public string nazwa { get; set; }
        public ICollection<Rasa> Rasa { get; set; }
        public Gatunek() { }
    }
}
