namespace Cwiczenia3;

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