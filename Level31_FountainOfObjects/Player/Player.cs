
using static FountainOfObjectsGame;


public class Player : IPlayer
{
    private readonly IUserInteractor _userInteractor;
    private int _maxHorizontolRange = 0;
    private int _maxVerticalRange = 0;

    public Player(IUserInteractor userInteractor)
    {
        _userInteractor = userInteractor;

    }

    private Position _position;
    public Position Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }

    public void SetVeriticalAndHorizontalRange(int range)
    {
        _maxHorizontolRange = range;
        _maxVerticalRange = range;
    }


    public int NumberOfArrows { get; private set; } = 5;

    public void ShootArrow()
    {
        if (NumberOfArrows == 0)
        {
            _userInteractor.ShowMessageNewLine("You have no more arrows!");
        }
        else
        {
            _userInteractor.ShowMessageNewLine("You shot an arrow");
            NumberOfArrows--;
        }
    }



    public bool MoveEast()
    {

        if (canMoveEast())
        {
            _position.Column++;
            return true;
        }

        return false;
    }

    public bool MoveWest()
    {
        if (canMoveWest())
        {
            _position.Column--;
            return true;
        }

        return false;
    }

    public bool MoveSouth()
    {
        if (canMoveSouth())
        {
            _position.Row--;
            return true;
        }

        return false;
    }

    public bool MoveNorth()
    {
        if (canMoveNorth())
        {
            _position.Row++;
            return true;
        }

        return false;
    }

    private bool canMoveNorth()
    {
        return _position.Row < _maxVerticalRange - 1;
    }

    private bool canMoveSouth()
    {
        return _position.Row > 0;
    }

    private bool canMoveEast()
    {
        return _position.Column < _maxHorizontolRange - 1;
    }

    private bool canMoveWest()
    {
        return _position.Column > 0;
    }

}
