
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
    private IsHazardNotifier _isHazardNotifierImplementation;
    public bool IsHazardNotified { get; set; }
    public LiquidContainer(double wysokosc, double wagaWlasna, double glebokosc, double maxLadownosc, bool isHazardNotified) 
        : base("L", wysokosc, wagaWlasna, glebokosc, maxLadownosc)
    {
        IsHazardNotified = isHazardNotified;
    }

    public override void LoadContainer(double masa)
    {
        double procentWypelnienia = IsHazardNotified ? MaxLadownosc * 0.5 : MaxLadownosc * 0.9;
        if (masa > procentWypelnienia-MasaLadunku)
        {
            Notify($"ALERT: {NumerSeryjny}");
            
        }
        base.LoadContainer(masa);
    }
    
    public void Notify(string msg)
    {
        Notify(msg);
        throw new InvalidOperationException("Przeladowanie: Przekroczono limit masy kontenera!");
    }
}

public class GazContainer : Container, IsHazardNotifier
{
    public double Cisnienie { get; set; }
    public bool IsHazardNotified { get; set; }
    
    public GazContainer(double cisnienie, double wysokosc, double wagaWlasna, double glebokosc, double maxLadownosc, bool isHazardNotified) 
        : base("G", wysokosc, wagaWlasna, glebokosc, maxLadownosc)
    {
        IsHazardNotified = isHazardNotified;
        Cisnienie = cisnienie;
    }
    public override void EmptyContainer()
    {
        MasaLadunku = MasaLadunku * 0.05;
    }
    public void Notify(string msg)
    {
        Notify(msg);
        throw new InvalidOperationException("Przeladowanie: Przekroczono limit masy kontenera!");
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
        Notify(msg);
        throw new InvalidOperationException("Przeladowanie: Przekroczono limit masy kontenera!");
    }
}