using System.ComponentModel.DataAnnotations;

namespace Adopcja_2024.Models
{
    public class Instytucja
    {
        [Key]
        public int id_instytucja { get; set; }

        [Display(Name = "Nazwa Instytucji")]
        //[Required(ErrorMessage = "Nazwa instytucji jest wymagana!")]
        [DataType(DataType.Text)]
        public string nazwa { get; set; }

        //[Required(ErrorMessage = "Miasto jest wymagane!")]
        [DataType(DataType.Text)]
        public int id_miasto { get; set; }
        public Miasto? Miasto { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Ulica zaczyna się z wielkiej litery")]
        public string ulica { get; set; }

        //[Required(ErrorMessage = "Kod pocztowy jest wymagany!")]
        [DataType(DataType.Text)]
        public string kod_pocztowy { get; set; }

        //[Required(ErrorMessage = "Numer domu jest wymagany!")]
        [DataType(DataType.Text)]
        public string nr_domu { get; set; }

        public int? nr_lokalu { get; set; }

        public string nr_telefonu { get; set; }

        public string e_mail { get; set; }

        public Osoba? Osoba { get; set; }

        public Instytucja()
        {

        }
    }
}
