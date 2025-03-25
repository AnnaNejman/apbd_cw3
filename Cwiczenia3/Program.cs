
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

        Container c1 = new LiquidContainer(3, 1, 3, 50, false);
        Container c2 = new GazContainer(100,5,1,5,60);
        Container c3 = new RefrigeratedContainer(20, 1, 5, 5, "Bananas", 20);
        
        c1.LoadContainer(6);
        c2.LoadContainer(2);

        ship1.AddContainer(c1);
        ship2.AddContainer(c2);
        
        List<Container> containers = new List<Container>();
        containers.Add(c2);
        containers.Add(c3);
        containers.Add(c1);
        
        ship1.PrintShipInfo();
        ship1.ReplaceContainer(c1.NumerSeryjny, c2);
        
        ship1.MoveContainer(c2,ship2);
        ship2.PrintContainerInfo(c1.NumerSeryjny);
        
        ship1.AddContainer(containers);
        
        ship1.PrintShipInfo();
        ship2.PrintShipInfo();
        
    }
}


