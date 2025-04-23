using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adopcja_2024.Models
{
    public class Ogloszenie
    {
        [Key]
        public int? id_ogloszenie { get; set; }

        [Required(ErrorMessage = "Tytuł ogłoszenia jest wymagany!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,200}", ErrorMessage = "Imię zaczyna się z wielkiej litery")]
        [Display(Name = "Tytuł")]
        public string tytul { get; set; }

        [Required(ErrorMessage = "Data jest wymagana!")]
        [Display(Name = "Data")]
        [DataType(DataType.DateTime)]
        public DateTime data { get; set; }

        [Required(ErrorMessage = "Opis ogłoszenia jest wymagany!")]
        [DataType(DataType.Text)]
        [Display(Name = "Opis")]
        public string opis { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Zdjęcie ogłoszenia")]

        public byte[]? zdjecie { get; set; }

        [ForeignKey("Osoba")]
        public int? id_osoba { get; set; }

        public Osoba? Osoba { get; set; }

        public Ogloszenie() { }
    }
}
