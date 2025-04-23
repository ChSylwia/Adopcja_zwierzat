using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adopcja_2024.Models
{
    public class Rasa
    {
        [Key]
        public int id_rasa { get; set; }
        public string nazwa { get; set; }
        [ForeignKey("Gatunek")]
        public int id_gatunek { get; set; }
        public ICollection<Zwierzak> Zwierzak { get; set; }
        public Gatunek Gatunek { get; set; }
        public Rasa() { }
    }
}
