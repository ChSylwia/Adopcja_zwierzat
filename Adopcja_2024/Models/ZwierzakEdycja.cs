using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adopcja_2024.Models
{
    public class ZwierzakEdycja
    {
        [Key]
        public int? id_zwierzak { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Imię zaczyna się z wielkiej litery")]
        [Display(Name = "Imie")]
        public string imie { get; set; }

        [Required(ErrorMessage = "Płeć jest wymagana!")]
        [DataType(DataType.Text)]
        public bool czy_samiczka { get; set; }

        public int wiek { get; set; }

        [Required(ErrorMessage = "Wielkość jest wymagana!")]
        public int wielkosc { get; set; }
        [ForeignKey("Rasa")]
        public int id_rasa { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany!")]
        [DataType(DataType.Text)]
        public string opis { get; set; }
        public string? zdjecie { get; set; }
        public Rasa Rasa { get; set; }
        public ZwierzakEdycja() { }
    }
}
