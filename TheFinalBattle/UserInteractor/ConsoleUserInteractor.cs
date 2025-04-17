public class ConsoleUserInteractor : IUserInteractor
{
    public void Write(string message, TextColor textColor = TextColor.Black)
    {
        Console.ForegroundColor = (ConsoleColor)textColor;
        Console.Write(message);
    }

    public string GetUserInput()
    {
        Console.ForegroundColor = (ConsoleColor)TextColor.Cyan;

        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return "";
        }

        return input;

    }

    public void WriteLine(string message, TextColor textColor = TextColor.Black)
    {
        //Thread.Sleep(50);
        Console.ForegroundColor = (ConsoleColor)textColor;
        Console.WriteLine(message);
    }

    public void ChangeTextColor(TextColor textColor)
    {
        Console.ForegroundColor = (ConsoleColor)textColor;
    }

    public string GetUserInput(string prompt)
    {

        Console.WriteLine(prompt);

        Console.ForegroundColor = (ConsoleColor)TextColor.Cyan;
        
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return "";
        }

        return input;


    }
}


public enum TextColor
{
    Black = 0,
    DarkBlue = 1,
    DarkGreen = 2,
    DarkCyan = 3,
    DarkRed = 4,
    DarkMagenta = 5,
    DarkYellow = 6,
    Gray = 7,
    DarkGray = 8,
    Blue = 9,
    Green = 10,
    Cyan = 11,
    Red = 12,
    Magenta = 13,
    Yellow = 14,
    White = 15
}