public interface IPlayer
{
    int NumberOfArrows { get; }
    Position Position { get; set; }

    bool MoveEast();
    bool MoveNorth();
    bool MoveSouth();
    bool MoveWest();
    void SetVeriticalAndHorizontalRange(int range);
    void ShootArrow();
}