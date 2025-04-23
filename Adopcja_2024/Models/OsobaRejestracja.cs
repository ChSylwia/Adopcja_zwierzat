using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adopcja_2024.Models
{
    public class OsobaRejestracja
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
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Nazwisko zaczyna się z wielkiej litery")]
        public string nazwisko { get; set; }

        public bool czy_kobieta { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane!")]
        [DataType(DataType.Text)]
        public string nazwa_miasta { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[0-9]{9}", ErrorMessage = "Numer Telefonu w formacie 000000000")]
        public string nr_telefonu { get; set; }
        public int? id_instytucja { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany!")]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [DataType(DataType.Password)]
        public string haslo { get; set; }

        public bool czy_instytucja { get; set; }

        //[Required(ErrorMessage = "Nazwa instytucji jest wymagana!")]
        [DataType(DataType.Text)]
        public string? nazwa_instytucji { get; set; }

        //[Required(ErrorMessage = "Miasto jest wymagane!")]
        [DataType(DataType.Text)]
        public string? miasto { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Ulica zaczyna się z wielkiej litery")]
        public string? ulica { get; set; }

        //[Required(ErrorMessage = "Kod pocztowy jest wymagany!")]
        [DataType(DataType.Text)]
        public string? kod_pocztowy { get; set; }

        //[Required(ErrorMessage = "Numer domu jest wymagany!")]
        [DataType(DataType.Text)]
        public string? nr_domu { get; set; }

        public int? nr_lokalu { get; set; }

        //[Required(ErrorMessage = "Numer telefonu jest wymagany!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[0-9]{9}", ErrorMessage = "Numer Telefonu w formacie 000000000")]
        public string? nr_telefonu_instytucja { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        public string? email_instytucja { get; set; }

        public OsobaRejestracja()
        {

        }
    }
}
