


public interface IUserInteractor
{
    void Write(string message, TextColor textColor = TextColor.White);
    void WriteLine(string message, TextColor textColor = TextColor.White);
    string GetUserInput();
    string GetUserInput(string prompt);
    void ChangeTextColor(TextColor white);
}
