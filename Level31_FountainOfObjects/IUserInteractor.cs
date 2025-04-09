


















public interface IUserInteractor
{
    void ShowMessage(string message, TextColor textColor = TextColor.White);
    void ShowMessageNewLine(string message, TextColor textColor = TextColor.White);
    string GetUserInput();
    void ChangeTextColor(TextColor white);
}
