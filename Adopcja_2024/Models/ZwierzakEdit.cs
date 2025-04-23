namespace Adopcja_2024.Models
{
    public class ZwierzakEdit
    {
        public Zwierzak Zwierzak { get; set; }
        public IEnumerable<Gatunek> Gatunek { get; set; }
        public IEnumerable<Rasa> Rasa { get; set; }
        public IEnumerable<Wielkosc> Wielkosc { get; set; }
        public IEnumerable<Wiek> Wiek { get; set; } 

        public ZwierzakEdit() { }
    }
}
