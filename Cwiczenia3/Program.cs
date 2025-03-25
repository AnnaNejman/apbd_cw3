
using Cwiczenia3;

public abstract class Container
{
    private static int counter = 0;
    public double MasaLadunku { get; set; }
    public double Wysokosc { get; set; }
    public double WagaWlasna { get; set; }
    public double Glebokosc { get; set; }
    public string NumerSeryjny { get; set; }
    public double MaxLadownosc { get; set; }

    public Container(string type, double wysokosc, double wagaWlasna, double glebokosc, double maxLadownosc)
    {
        int newNumber = ++counter;
        MasaLadunku = 0;
        Wysokosc = wysokosc;
        WagaWlasna = wagaWlasna;
        Glebokosc = glebokosc;
        MaxLadownosc = maxLadownosc;
        NumerSeryjny = $"KON-{type}-{newNumber}";
    }

    public virtual void EmptyContainer()
    {
        MasaLadunku = 0;
    }

    public virtual void LoadContainer(double masa)
    {
        if (MasaLadunku + masa > MaxLadownosc)
        {
            throw new InvalidOperationException("Przekroczono maksymalną ładowność kontenera!");
        }
        MasaLadunku += masa;
    }
    public void PrintContainerInfo()
    {
        Console.WriteLine($"Kontener: {NumerSeryjny}, Zaladowano: {MasaLadunku} kg, Max masa ładunku: " +
                          $"{MaxLadownosc} kg, Masa własna: {WagaWlasna} kg, Wysokość: {Wysokosc} cm, Głębokość: {Glebokosc} cm");
    }
}

public interface IsHazardNotifier
{
    void Notify(string msg);
}
class Program
{
    static void Main()
    {
        ContainerShip ship1 = new ContainerShip("s1", 50, 10, 100);
        ContainerShip ship2 = new ContainerShip("s2", 25, 12, 200);

        Container c1 = new LiquidContainer(3, 1, 3, 500, false);
        Container c2 = new GazContainer(100,5,1,5,600);
        Container c3 = new RefrigeratedContainer(20, 1, 5, 5, "Bananas", 20);
        
        c1.PrintContainerInfo();
        c1.LoadContainer(25);
        c1.PrintContainerInfo();
        
        ship1.PrintShipInfo();
        ship2.PrintShipInfo();
        
        c1.PrintContainerInfo();
        c2.PrintContainerInfo();
        c3.PrintContainerInfo();
        
        Console.WriteLine("Dodaj c1 do ship1");
        ship1.AddContainer(c1);
        ship1.PrintShipInfo();
        
        Console.WriteLine("Dodaj do ship2 liste kontenerów");
        List<Container> containers = new List<Container>();
        containers.Add(c1);
        containers.Add(c2);
        containers.Add(c3);
        
        ship2.AddContainer(containers);
        ship2.PrintShipInfo();
        
    }
}


