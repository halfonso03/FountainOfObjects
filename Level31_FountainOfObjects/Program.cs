

new FountainOfObjectsGame(
        new ConsoleUserInteractor(), 
        new GameboardRenderer(),
        new Player(new ConsoleUserInteractor())).Start();





public class FountainOfObjectsGame
{
    private readonly IUserInteractor _userInteractor;
    private readonly IGameboardRenderer _gameboardRenderer;
    private readonly IPlayer _player;
    private const string USER_PROMPT_MESSAGE = "What do you want to do? ";
    private GameBoard _gameBoard;
    private Dictionary<string, Func<bool>> _playerMoves;
    private Position _startingPosition = new Position();
    private Position _fountainPosition = new() { Column = 2, Row = 0 };
    private Position _entrancePosition = new();
    private Position _playerPosition;
    private FountainState _fountainState = FountainState.Disabled;
    private string[] _validCommands = [];

    public FountainOfObjectsGame(IUserInteractor userInteractor, 
            IGameboardRenderer gameboardRenderer,
            IPlayer player)
    {
        _playerPosition = _startingPosition;
        _userInteractor = userInteractor;
        _gameboardRenderer = gameboardRenderer;
        _player = player;   
        _validCommands = ["me", "mn", "ms", "mw", "se", "sw", "ss", "sn", "ef", "df"];
    }

    public void Start()
    {
        int boardSize = ChooseBoardSize();

        _player.SetVeriticalAndHorizontalRange(boardSize);
        _gameBoard = _gameboardRenderer.RenderGameBoard(boardSize);

        _userInteractor.ChangeTextColor(TextColor.White);
        _userInteractor.ShowMessageNewLine("");
        _userInteractor.ShowMessageNewLine("");

        
        _playerMoves = new Dictionary<string, Func<bool>>
        {
            { "me", _player.MoveEast },
            { "mw", _player.MoveWest },
            { "ms", _player.MoveSouth },
            { "mn", _player.MoveNorth },
            { "sn", () => ShootArrowAtAmarok( _playerPosition.Row - 1, _playerPosition.Column)},
            { "ss", () => ShootArrowAtAmarok( _playerPosition.Row - 1, _playerPosition.Column) },
            { "se", () => ShootArrowAtAmarok( _playerPosition.Row, _playerPosition.Column + 1) },
            { "sw", () => ShootArrowAtAmarok( _playerPosition.Row, _playerPosition.Column - 1) },
            { "ef", () =>
            {
                if (UserLocationIsFountainLocation)
                {
                    if (_fountainState == FountainState.Disabled)
                    {
                        _fountainState = FountainState.Enabled;
                    }
                    else if (_fountainState == FountainState.Enabled)
                    {
                        _userInteractor.ShowMessageNewLine("Fountain is already on!");
                    }
                }
                else
                {
                    _userInteractor.ShowMessageNewLine("You haven't found the fountain yet!", TextColor.Red);
                }
                return true;
            } },
            { "df", () =>
            {
                if (UserLocationIsFountainLocation)
                {
                    if (_fountainState == FountainState.Enabled)
                    {
                        _fountainState = FountainState.Disabled;
                    }
                    else if (_fountainState == FountainState.Disabled)
                    {
                        _userInteractor.ShowMessageNewLine("Fountain is already off!");
                    }
                }
                else
                {
                    _userInteractor.ShowMessageNewLine("You haven't found the fountain yet!", TextColor.Red);
                }
                return true;
            } }
        };

        BeginGame();
    }

    private bool ShootArrowAtAmarok(int row, int column)
    {
        if (_player.NumberOfArrows > 0) 
        {
            _player.ShootArrow();

            var shootingPosition = new Position(row, column);

            var killed = KilledAmarokAtPosition(shootingPosition);

            _userInteractor.ShowMessageNewLine($"You shot an arrow to {shootingPosition}. You have {_player.NumberOfArrows} arrows left", TextColor.Green); 

            if (!killed.HasValue)
            {
                _userInteractor.ShowMessageNewLine("You shot an arrow but there was nothing to kill...");
            }
            else if (killed.HasValue && killed.Value == true)
            {
                _userInteractor.ShowMessageNewLine("You killed an Amarok! :)");
            }
            else if (killed.HasValue && killed.Value == false)
            {
                _userInteractor.ShowMessageNewLine("You shot a dead Amarok :|");
            }
        }
        else
        {
            _userInteractor.ShowMessageNewLine("You have nomor arrows!");
        }
        

        return true;
    }

    private bool? KilledAmarokAtPosition(Position position)
    {
        var obstacle = _gameBoard.GetObstacleAtPosition(position) as Amarok;

        if (obstacle != null)
        {
            if (obstacle.IsAlive)
            {
                obstacle.IsAlive = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        return null;
    }

    private int ChooseBoardSize()
    {
        do
        {
            _userInteractor.ShowMessageNewLine("Chooe a board size 4, 6 or 8");

            string input = _userInteractor.GetUserInput();

            if (int.TryParse(input, out var choice))
            {
                return choice;
            }
            else
            {
                _userInteractor.ShowMessageNewLine("Invalid size.");
            }
        }
        while (true);
    }

    private void BeginGame()
    {
        var gameWon = false;
        var gameLost = false;

        do
        {
            ShowPositionMessage();


            if (UserLocationIsCavernEntrance)
            {
                _userInteractor.ShowMessageNewLine("You see light coming from the cavern entrance.", TextColor.Yellow);
            }

            if (UserLocationIsFountainLocation)
            {
                if (_fountainState == FountainState.Disabled)
                {
                    _userInteractor.ShowMessageNewLine("You hear water dripping from this room. The Fountain of Objects is here!", TextColor.Blue);
                }

                if (_fountainState == FountainState.Enabled)
                {
                    _userInteractor.ShowMessageNewLine("You hear rushing waters rom the Fountain of Objects. It has been reactivated!", TextColor.Blue);
                }
            }



            Obstacle[] nearByObstacles = _gameBoard.GetNearestObstacles(_player.Position);

            foreach (var obstacle in nearByObstacles)
            {
                if (obstacle is Amarok amarok)
                {
                    if (amarok.IsAlive)
                    {
                        _userInteractor.ShowMessageNewLine(obstacle.Message, TextColor.DarkRed);
                    }
                }
                else
                {
                    _userInteractor.ShowMessageNewLine(obstacle.Message, TextColor.DarkRed);
                }
                
            }


            _userInteractor.ShowMessage(USER_PROMPT_MESSAGE, TextColor.White);

            var command = _userInteractor.GetUserInput();

            if (IsValidInput(command))
            {
                if (_playerMoves[command]())
                {
                    //_playerPosition = _gameBoard.PlayerPosition;
                    _playerPosition = _player.Position;

                    if (_gameBoard.PlayerLandedOnObstacle<Pit>(_playerPosition))
                    {
                        gameLost = true;
                    }
                    else if (_gameBoard.PlayerLandedOnObstacle<Amarok>(_playerPosition))
                    {
                        var amarok = _gameBoard.GetObstacleAtPosition(_playerPosition) as Amarok;

                        if (amarok!.IsAlive)
                        {
                            gameLost = true;
                        }
                    }
                    else if (_gameBoard.PlayerLandedOnObstacle<Maelstrom>(_playerPosition)) {

                        _gameBoard.MovePlayerToPosition(_playerPosition.Row + 1, _playerPosition.Column + 2);

                        _userInteractor.ShowMessageNewLine("The maelstrom has pushed you away! Try again and be careful!");

                        var maelstrom = _gameBoard.GetObstacleAtPosition(_playerPosition);

                        if (maelstrom != null)
                        {
                            _gameBoard.MoveObstacleToPosition(maelstrom, maelstrom.Position.Row - 1, maelstrom.Position.Column + 2);
                        }

                        //_playerPosition = _gameBoard.PlayerPosition;
                        _playerPosition = _player.Position;

                        Obstacle? otherObstacle = _gameBoard.GetObstacleAtPosition(_playerPosition);

                        while (otherObstacle is not null)
                        {
                            _gameBoard.MovePlayerToPosition(_playerPosition.Row + 1, _playerPosition.Column + 1);
                            //_playerPosition = _gameBoard.PlayerPosition;
                            _playerPosition = _player.Position;
                            otherObstacle = _gameBoard.GetObstacleAtPosition(_playerPosition);
                        }
                    }
                }
                else
                {
                    _userInteractor.ShowMessageNewLine("Invalid move. You may move east(me), move west(mw), move north(mn), mouth south(ms).", TextColor.Red);
                }
            }
            else
            {
                _userInteractor.ShowMessageNewLine("Invalid input entered! Try again.", TextColor.Red);
            }

            gameWon = UserLocationIsCavernEntrance && _fountainState == FountainState.Enabled && !gameLost;


        } while (!gameWon && !gameLost);


        if (gameWon)
        {
            _userInteractor.ShowMessageNewLine("---------------------------------------------------------", TextColor.DarkGreen);
            _userInteractor.ShowMessageNewLine("The Fountain of Objects has been reactivated, you have escaped with your life!", TextColor.DarkYellow);
            _userInteractor.ShowMessageNewLine("You Win!", TextColor.DarkYellow);
        }

        if (gameLost)
        {
            _userInteractor.ShowMessageNewLine("YOU LOST!", TextColor.DarkYellow);
        }
    }

    private bool UserLocationIsFountainLocation =>
        _playerPosition.Row == _fountainPosition.Row && _playerPosition.Column == _fountainPosition.Column;

    private bool UserLocationIsCavernEntrance =>
        _playerPosition.Row == _entrancePosition.Row && _playerPosition.Column == _entrancePosition.Column;

    private bool IsValidInput(string command) => _validCommands.Contains(command);
       
    private void ShowPositionMessage()
    {
        _userInteractor.ShowMessageNewLine("---------------------------------------------------------", TextColor.DarkGreen);
        _userInteractor.ShowMessageNewLine($"You are in the room at (Row={_playerPosition.Row} Column={_playerPosition.Column})", TextColor.White);
        _userInteractor.ShowMessageNewLine($"You have {_player.NumberOfArrows} arrows remaining.");
    }
}




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