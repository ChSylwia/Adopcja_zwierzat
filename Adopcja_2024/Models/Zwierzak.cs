using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adopcja_2024.Models
{
    public class Zwierzak
    {
        [Key]
        public int id_zwierzak { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Imię zaczyna się z wielkiej litery")]
        [Display(Name = "Imie")]
        public string imie { get; set; }
        [Display(Name = "Płeć")]
        [Required(ErrorMessage = "Płeć jest wymagana!")]
        [DataType(DataType.Text)]
        public bool czy_samiczka { get; set; }
        [Display(Name = "Wiek")]
        [ForeignKey("Wiek")]
        public int id_wiek { get; set; }
        //public string wiek { get; set; }
        [Required(ErrorMessage = "Wielkość jest wymagana!")]
        [DataType(DataType.Text)]
        //public string wielkosc { get; set; }
        [Display(Name = "Wielkość")]
        [ForeignKey("Wielkosc")]
        public int id_wielkosc { get; set; }
        
        [Required(ErrorMessage = "Rasa jest wymagana!")]
        [DataType(DataType.Text)]
        //public string rasa { get; set; }
        [Display(Name = "Rasa")]
        [ForeignKey("Rasa")]
        public int id_rasa { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany!")]
        [Display(Name = "Opis zwierzaka")]
        [DataType(DataType.Text)]
        public string opis { get; set; }
        [Display(Name = "Zdjęcie zwierzaka")]
        [DataType(DataType.Upload)]
        public byte[]? zdjecie { get; set; }
        public bool czy_adoptowany { get; set; }
        [ForeignKey("Osoba")]
        public int? id_osoby { get; set; }
        public Osoba? Osoba { get; set; }
        public Rasa? Rasa { get; set; }
        public Wiek? Wiek { get; set; }
        public Wielkosc? Wielkosc { get; set; }
        public  Rezerwacja? Rezerwacja { get; set; }
        
        public Zwierzak() { }
    }
}
