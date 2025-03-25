namespace Cwiczenia3;

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