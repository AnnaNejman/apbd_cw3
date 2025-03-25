
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
public class LiquidContainer : Container, IsHazardNotifier
{
    public bool IsHazardNotified { get; set; }
    public LiquidContainer(double wysokosc, double wagaWlasna, double glebokosc, double maxLadownosc, bool isHazardNotified) 
        : base("L", wysokosc, wagaWlasna, glebokosc, maxLadownosc)
    {
        IsHazardNotified = isHazardNotified;
    }

    public override void LoadContainer(double masa)
    {
        double procentWypelnienia = IsHazardNotified ? MaxLadownosc * 0.5 : MaxLadownosc * 0.9;
        if (masa > procentWypelnienia - MasaLadunku)
        {
            Console.WriteLine($"ALERT: {NumerSeryjny}");
        }
        base.LoadContainer(masa);
    }
    
    public void Notify(string msg)
    {

        throw new InvalidOperationException(msg+ "Przeladowanie: Przekroczono limit masy kontenera!");
    }
}

public class GazContainer : Container, IsHazardNotifier
{
    public double Cisnienie { get; set; }
    
    public GazContainer(double cisnienie, double wysokosc, double wagaWlasna, double glebokosc, double maxLadownosc) 
        : base("G", wysokosc, wagaWlasna, glebokosc, maxLadownosc)
    {
        Cisnienie = cisnienie;
    }
    public override void EmptyContainer()
    {
        MasaLadunku = MasaLadunku * 0.05;
    }
    public void Notify(string msg)
    {
        throw new InvalidOperationException(msg + " Przeladowanie: Przekroczono limit masy kontenera!");
    }
    public override void LoadContainer(double masa)
    {
        if (masa > MaxLadownosc-MasaLadunku)
        {
            Notify($"ALERT: {NumerSeryjny}");
        }
        base.LoadContainer(masa);
    }
}

public class RefrigeratedContainer : Container, IsHazardNotifier
{
    public double Temperature { get; set; }
    public string ProductType{ get; set; }

    private static Dictionary<string, double> ProduktMintemperature = new Dictionary<string, double>
    {
        { "Bananas", 13.3 },
        { "Chocolate", 18 },
        { "Fish", 2 },
        { "Meat", -15 },
        { "Ice cream", -18 },
        { "Frozen pizza", -30 },
        { "Cheese", 7.2 },
        { "Sausages", 5 },
        { "Butter", 20.5 },
        { "Eggs", 19 },
    };
    public bool IsHazardNotified { get; set; }
    
    
    public RefrigeratedContainer(double maxCapacity, double ownWeight, double height, double depth, string productType, double temperature)
        : base("C", maxCapacity, ownWeight, height, depth)
    {
        if (!ProduktMintemperature.ContainsKey(productType))
        {
            throw new InvalidOperationException("Nie mozna transportowac tego produktu: " + productType);
        }
        if (ProduktMintemperature.ContainsKey(productType) && temperature < ProduktMintemperature[productType])
        {
            throw new InvalidOperationException($"Temperatura jest zbyt niska dla {ProductType}");
        }
        ProductType = productType;
        Temperature = temperature;
    }

    public void Notify(string msg)
    {
        throw new InvalidOperationException(msg);
    }
}

public class ContainerShip
{
    public string Name;
    private List<Container> Containers { get; set; }
    public double Speed { get; set; }
    public int MaxContainersNumber { get; set; }
    public double MaxWeight { get; set; }

    public ContainerShip(string name, double speed, int maxContainersNumber, double maxWeight)
    {
        Name = name;
        Speed = speed;
        MaxContainersNumber = maxContainersNumber;
        MaxWeight = maxWeight;
        Containers = new List<Container>();
    }

    public void AddContainer(Container container)
    {
        if (Containers.Count >= MaxContainersNumber)
        {
            throw new Exception("Kontenerowiec jest juz pelny!");
        }
        
        if (GetCurrentLoad()+container.MasaLadunku+container.WagaWlasna > MaxWeight)
        {
            throw new Exception("Masa kontenera jest zbyt duza zeby zaladowac go na ten kontenerowiec!");
        }
        Containers.Add(container);
    }
    public void AddContainer(List<Container> containers)
    {
        for (int i = 0; i < containers.Count; i++)
        {
            AddContainer(containers[i]);
        }
    }

    public void RemoveContainer(string serialNumber)
    {
        var element = Containers.RemoveAll(c => c.NumerSeryjny == serialNumber);
        if (element == 0)
        {
            throw new Exception($"Nie znaleziono elementu z numerem seryjnym: {serialNumber}");
        }
    }
    public void ReplaceContainer(string serialNumber, Container newContainer)
    {
        RemoveContainer(serialNumber);
        AddContainer(newContainer);
    }

    public void MoveContainer(Container container,ContainerShip newContainerShip)
    {
        RemoveContainer(container.NumerSeryjny);
        newContainerShip.AddContainer(container);
    }
    public void PrintContainerInfo(string serialNumber)
    {
        var container = Containers.Find(c => c.NumerSeryjny == serialNumber);
        if (container != null)
        {
            Console.WriteLine($"Kontener: {container.NumerSeryjny}, Zaladowano: {container.MasaLadunku} kg, Max masa ładunku: " +
                              $"{container.MaxLadownosc} kg, Masa własna: {container.WagaWlasna} kg, Wysokość: {container.Wysokosc} cm, Głębokość: {container.Glebokosc} cm");
        }
        else
        {
            Console.WriteLine("Nie znaleziono tego kontenera.");
        }
    }
    public void PrintShipInfo()
    {
        Console.WriteLine($"Kontenerowiec: {Name}, Max waga ładunku: {MaxWeight} ton, Max liczba kontenerów: {MaxContainersNumber}, Załaodowano: " +
                          $"{GetCurrentLoad()} ton, Liczba kontenerów załadowanych: {Containers.Count}");
    }

    public double GetCurrentLoad()
    {
        return Containers.Sum(c => c.MasaLadunku + c.WagaWlasna);
    }
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


