


var rs = new RepeatingStream();


Thread thread1 = new Thread(rs.Start);
thread1.Start();

do
{
    var keyPressed = Console.ReadKey();

    if (keyPressed.Key == ConsoleKey.Enter)
    {
        if (rs.RecentNumbers.Number1== rs.RecentNumbers.Number2)
        {
            Console.WriteLine("Youve identified repeating numbers;");
        }
        else
        {
            Console.WriteLine("error");
        }
    }

    

} while (true);



Console.ReadKey();

void CountTo100()
{
    for (int i = 1; i <= 100; i++)
        Console.Write(i);

    Console.WriteLine("Finishe");
}