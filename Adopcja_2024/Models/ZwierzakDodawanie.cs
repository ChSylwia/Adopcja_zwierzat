namespace Adopcja_2024.Models
{
    public class ZwierzakDodawanie
    {
        public IEnumerable<Zwierzak> zwierzaki { get; set; }
        public IEnumerable<Rasa> rasa { get; set; }
        public IEnumerable<Gatunek> gatunek { get; set; }
        public IEnumerable<Wiek> wiek { get; set; }
        public IEnumerable<Wielkosc> wielkosc { get; set; }
        public ZwierzakDodawanie() { }
    }
}
