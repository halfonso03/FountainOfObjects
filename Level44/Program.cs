

string? word;

do
{
    Console.Write("Enter a word to randomly regenerate: ");


    word = Console.ReadLine();

    Task.Run(() => Go(word));
}
while (word != "exit");



async void Go(string word)
{
    DateTime start = DateTime.Now;
    int attempts = await RandomlyRecreateAsync(word);
    Console.WriteLine(attempts);
    TimeSpan elapsed = DateTime.Now - start;
    Console.WriteLine(elapsed);
}



int RandomlyRecreate(string? word)
{
    if (word == null) return 0;

    Random random = new Random();

    string generated;
    int attempts = 0;
    do
    {
        attempts++;
        generated = "";
        for (int letter = 0; letter < word.Length; letter++)
            generated += (char)('a' + random.Next(26));
    } while (generated != word);

    return attempts;
}

Task<int> RandomlyRecreateAsync(string? word)
{
    return Task.Run(() => RandomlyRecreate(word));
}


//AsyncMethod();

//async Task AsyncMethod()
//{
//    Console.WriteLine("A");
//    Task task = Task.Run(() => {
//        Thread.Sleep(1000);
//        Console.WriteLine("B"); 
//    });
//    Console.WriteLine("C");
//    //await task;
//    Console.WriteLine("D");


//}

/*
 * A
 * C
 * B
 * D
 * */

Console.ReadLine();
