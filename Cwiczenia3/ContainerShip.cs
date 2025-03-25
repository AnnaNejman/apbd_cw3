namespace Cwiczenia3;

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
        
        if (GetCurrentLoad()+container.MasaLadunku+container.WagaWlasna > MaxWeight*1000)
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
    public void PrintContainerInfo(Container container)
    {
        Console.WriteLine($"Kontener: {container.NumerSeryjny}, Zaladowano: {container.MasaLadunku} kg, Max masa ładunku: " +
                              $"{container.MaxLadownosc} kg, Masa własna: {container.WagaWlasna} kg, Wysokość: {container.Wysokosc} cm, Głębokość: {container.Glebokosc} cm");
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