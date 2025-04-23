using System.ComponentModel.DataAnnotations;

namespace Adopcja_2024.Models
{
    public class User
    {
        public int? id_osoba { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany!")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [DataType(DataType.Password)]
        public string haslo { get; set; }

        public User()
        {

        }
    }
}
