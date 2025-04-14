
public class RepeatingStream
{
    public RecentNumbers RecentNumbers  { get; set; } =  new RecentNumbers();

    private object _lock = new object();    

    public void Start()
    {
   

        var random = new Random();

        do
        {
            var num1 = random.Next(0, 9);
            var num2 = random.Next(0, 9);

            lock (_lock)
            {
                RecentNumbers.Number1 = num1;
                RecentNumbers.Number2 = num2;
            }
            

            Console.WriteLine($"Num1: {num1}. Num2 {num2}");

            Thread.Sleep(1000);
        }
        while (true);
    }

}

public class RecentNumbers
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
}
