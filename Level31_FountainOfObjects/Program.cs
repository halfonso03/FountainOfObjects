new FountainOfObjectsGame(
        new ConsoleUserInteractor(), 
        new GameboardRenderer(),
        new Player(new ConsoleUserInteractor())).Start();




public record Obstacle (string Message)
{
    public required Position Position { get; set; }
}

public record Pit (string message = "You feel a draft. There is a pit nearby") : Obstacle(message);

public record Maelstrom(string message = "You hear the growling and groaning of a maelstrom nearby") : Obstacle(message);

public record Amarok(string message = "You can smell the rotten stench of an amarok in a nearby room") : Obstacle(message)
{
    public bool IsAlive { get; set; } = true;
}


public enum FountainState
{
    Enabled,
    Disabled
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


public struct Position
{
    public int Row;
    public int Column;

    public Position(int row, int coloumn)
    {
        Row = row;
        Column = coloumn;
    }
}