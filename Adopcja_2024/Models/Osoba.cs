using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adopcja_2024.Models
{
    public class Osoba
    {
        [Key]
        public int id_osoba { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Imię zaczyna się z wielkiej litery")]
        [Display(Name = "Imie")]
        public string imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane!")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Nazwisko zaczyna się z wielkiej litery")]
        public string nazwisko { get; set; }
        [Display(Name = "Kobieta")]
        public bool czy_kobieta { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany!")]
        [DataType(DataType.Text)]
        [Display(Name = "Numer telefonu")]
        [RegularExpression(@"[0-9]{9}", ErrorMessage = "Numer Telefonu w formacie 000000000")]
        public string nr_telefonu { get; set; }
        public int? id_instytucja { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany!")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string email { get; set; }

        //[Required(ErrorMessage = "Hasło jest wymagane!")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string? haslo { get; set; }

        [ForeignKey("Miasto")]
        public int? id_miasto { get; set; }
        
        public Miasto Miasto { get; set; }
        public Instytucja? Instytucja { get; set; }

        public ICollection<Rezerwacja>? Rezerwacja { get; set; }
        public ICollection<Ogloszenie>? Ogloszenia { get; set; }

        public Osoba()
        {

        }
    }
}
