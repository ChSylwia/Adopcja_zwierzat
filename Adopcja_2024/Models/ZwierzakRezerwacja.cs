namespace Adopcja_2024.Models
{
    public class ZwierzakRezerwacja
    {
        public Zwierzak Zwierzak { get; set; }
        public IEnumerable<Rezerwacja> Rezerwacja { get; set; }
    }
}
