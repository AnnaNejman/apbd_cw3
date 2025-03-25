

public class Container
{
    private static int counter = 0;
    public double MasaLadunku { get; set; }
    public double Wysokosc { get; set; }
    public double WagaWlasna { get; set; }
    public double Glebokosc { get; set; }
    public String NumerSeryjny { get; set; }
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

    public void EmptyContainer()
    {
        MasaLadunku = 0;
    }

    public void LoadContainer(double masa)
    {
        if (MasaLadunku + masa > MaxLadownosc)
        {
            throw new InvalidOperationException("Przekroczono maksymalną ładowność kontenera!");
        }
        MasaLadunku += masa;
    }
    
}