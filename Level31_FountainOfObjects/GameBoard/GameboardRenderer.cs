using Level31_FountainOfObjects;
using Level31_FountainOfObjects.GameBoard;
using Level31_FountainOfObjects.GameBoard.Board;

public class GameboardRenderer() : IGameboardRenderer
{
    public IGameBoard RenderGameBoard(int boardSize)
    {
        int[,] grid = new int[boardSize, boardSize];
        Obstacle[] obstacles;

        if (boardSize == 6)
        {
            obstacles =
            [
                new Pit() { Position = new Position { Column = 2, Row = 3} },
                new Pit() { Position = new Position { Column = 5, Row = 4} },
                new Maelstrom() { Position = new Position { Column = 1, Row = 2} },
                new Amarok() { Position = new Position { Column = 3, Row = 4} },
            ];
        }
        else if (boardSize == 8)
        {
            obstacles =
            [
                new Pit() { Position= new Position { Column = 3, Row = 2 } },
                new Pit() { Position = new Position { Column = 4, Row = 5 } },
                new Pit() { Position = new Position { Column = 7, Row = 1 } },
                new Pit() { Position = new Position { Column = 3, Row = 4 } },
                new Maelstrom() { Position = new Position { Column = 1, Row = 2} },
                new Maelstrom() { Position = new Position { Column = 6, Row = 6} },
                new Amarok() { Position = new Position { Column = 2, Row = 7} },
                new Amarok() { Position = new Position { Column = 6, Row = 1} },
            ];
        }
        else
        {
            obstacles =
            [
                new Pit() { Position = new Position { Column = 3, Row = 1 } },
                new Maelstrom() { Position = new Position { Column = 1, Row = 2} },
                new Amarok() { Position = new Position { Column = 3, Row = 3} },
            ];
        }

        return new GameBoard(grid, obstacles);
    }
}
