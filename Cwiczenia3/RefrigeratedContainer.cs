namespace Cwiczenia3;

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