


















public class ConsoleUserInteractor : IUserInteractor
{
    public void ShowMessage(string message, TextColor textColor = TextColor.Black)
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

    public void ShowMessageNewLine(string message, TextColor textColor = TextColor.Black)
    {
        Console.ForegroundColor = (ConsoleColor)textColor;
        Console.WriteLine(message);
    }

    public void ChangeTextColor(TextColor textColor)
    {
        Console.ForegroundColor = (ConsoleColor)textColor;
    }
}
