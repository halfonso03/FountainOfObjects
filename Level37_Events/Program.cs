
CharberryTree charberryTree = new CharberryTree();

Notifier notifier =  new Notifier(charberryTree);

Harvester harvester = new Harvester(charberryTree);

while (true)
{
    charberryTree.MaybeGrow();
}


public class CharberryTree
{
    private Random _random = new Random();
    public bool Ripe { get; set; }

    public event Action? Ripened;


    public CharberryTree()
    {
        
    }

    public void MaybeGrow()
    {
        var nextDouble = _random.NextDouble();
        if (nextDouble < .00000001 && !Ripe)
        {
            Ripe = true;
            Ripened?.Invoke();
        }
    }
}

public class Notifier
{
    private readonly CharberryTree _charberryTree;

    public Notifier(CharberryTree charberryTree)
    {
        _charberryTree = charberryTree;
        _charberryTree.Ripened += HandleRipened; 
    }

    public void HandleRipened()
    {
        Console.WriteLine("A charberry fruit has ripened!");
    }
}

public class Harvester
{
    private readonly CharberryTree _charberryTree;

    public Harvester(CharberryTree charberryTree)
    {
        _charberryTree = charberryTree;
        _charberryTree.Ripened += HandleRipened;
    }

    public void HandleRipened()
    {
        _charberryTree.Ripe = false;
    }
}