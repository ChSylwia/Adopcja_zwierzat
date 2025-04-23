namespace Adopcja_2024.Models
{
    public class ZwierzakOgloszenia
    {
        public IEnumerable<Zwierzak> zwierzaki { get; set; }
        public IEnumerable<Ogloszenie> ogloszenia { get; set; }

        public ZwierzakOgloszenia()
        {

        }
    }
}
