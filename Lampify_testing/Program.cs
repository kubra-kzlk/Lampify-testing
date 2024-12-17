namespace Lampify_testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Lamp-klasse te instantiëren vanuit de Main-methode
            // Instantie van de Lamp-klasse maken

            Lamp lamp = new Lamp();

            // Lamp aanzetten
            lamp.TurnOn();
            Console.WriteLine($"Lamp is aan: {lamp.IsOn}"); // Verwacht: true

            // Helderheid instellen
            try
            {
                lamp.SetBrightness(75);
                Console.WriteLine($"Helderheid is ingesteld op: {lamp.Brightness}"); // Verwacht: 75
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Kleur instellen
            try
            {
                lamp.SetColor("Red");
                Console.WriteLine($"Kleur is ingesteld op: {lamp.Color}"); // Verwacht: Red
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Lamp uitzetten
            lamp.TurnOff();
            Console.WriteLine($"Lamp is aan: {lamp.IsOn}"); // Verwacht: false

            // Wacht op invoer voordat het programma sluit
            Console.WriteLine("Druk op een toets om af te sluiten...");
            Console.ReadKey();
        }
    }
}
