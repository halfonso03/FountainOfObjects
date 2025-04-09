


public interface IGameBoard
{

    Position CalculateNewPosition(int row, int column);

}



public class GameBoard : IGameBoard
{
    private int[,] _board { get; }

    private const double SQUARE_ROOT_OF_TWO = 1.41;
    private Position _playerPosition;
    private Obstacle[] _obstacles;

    public GameBoard(int[,] board, Obstacle[] obstacles)
    {
        _board = board;        
        _obstacles = obstacles;
    }


    public Position CalculateNewPosition(int row, int column)
    {
        if (row + 1 > _board.GetLength(0))
        {
            row = row % (_board.GetLength(0) - 1) - 1;
        }
        else if (row < 0)
        {
            do
            {
                row += _board.GetLength(0);
            }
            while (row < 0);
        }

        if (column + 1 > _board.GetLength(1))
        {
            column = column % (_board.GetLength(1) - 1) - 1;
        }
        else if (column < 0)
        {
            do
            {
                column += _board.GetLength(1);
            }
            while (column < 0);
        }

        return new Position(row, column);
    }

    public void MoveObstacleToPosition(Obstacle obstacle, int row, int column)
    {
        var translatedPosition = CalculateNewPosition(row, column);

        obstacle.Position = new Position(translatedPosition.Row, translatedPosition.Column);
    }

    public void MovePlayerToPosition(int row, int column)
    {
        var translatedPosition = CalculateNewPosition(row, column);

        _playerPosition.Row = translatedPosition.Row;
        _playerPosition.Column = translatedPosition.Column;
    }

 

    internal bool PlayerLandedOnObstacle<T>(Position playerPosition) where T : Obstacle
    {
        return _obstacles
            .Where(obstacle => obstacle is T)
            .Select(obstacle => GetDistanceBetweenPositions(playerPosition, obstacle.Position))
            .Any(distance => distance == 0);
    }

    internal Obstacle? GetObstacleAtPosition(Position playerPosition)
    {
        var tt = _obstacles
           .Where(obstacle => GetDistanceBetweenPositions(playerPosition, obstacle.Position) == 0)
           .Select(o => o)
           .FirstOrDefault();

        if (tt is not null)
        {
            return tt;
        }

        return null;

    }

    internal Obstacle[] GetNearestObstacles(Position playerPosition)
    {
        int obstacleCount = 0;
        Obstacle[] nearbyObstacles;

        foreach (var obstacle in _obstacles)
        {
            var distance = GetDistanceBetweenPositions(playerPosition, obstacle.Position);

            if (distance == 1)
            {
                obstacleCount++;
            }

            if (Math.Round(distance, 2) == SQUARE_ROOT_OF_TWO)
            {
                obstacleCount++;
            }
        }

        nearbyObstacles = new Obstacle[obstacleCount];
        obstacleCount = 0;

        foreach (var obstacle in _obstacles)
        {
            var distance = GetDistanceBetweenPositions(playerPosition, obstacle.Position);

            if (distance == 1 || Math.Round(distance, 2) == SQUARE_ROOT_OF_TWO)
            {
                nearbyObstacles[obstacleCount] = obstacle;
                obstacleCount++;
            }

        }

        return nearbyObstacles;
    }



    private static double GetDistanceBetweenPositions(Position position1, Position position2) =>
        Math.Sqrt(Math.Pow(position2.Column - position1.Column, 2) + Math.Pow(position2.Row - position1.Row, 2));


}
