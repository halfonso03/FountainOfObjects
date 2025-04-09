namespace Level31_FountainOfObjects.GameBoard.Board
{
    public interface IGameBoard
    {
        Position CalculateNewPosition(int row, int column);
        Obstacle[] GetNearestObstacles(Position playerPosition);
        Obstacle? GetObstacleAtPosition(Position playerPosition);
        void MoveObstacleToPosition(Obstacle obstacle, int row, int column);
        void MovePlayerToPosition(int row, int column);
        bool PlayerLandedOnObstacle<T>(Position playerPosition) where T : Obstacle;
    }
}