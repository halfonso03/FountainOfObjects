

using Level31_FountainOfObjects;
using System.Net.Http.Headers;

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
    private DateTime _startDateTime = new DateTime();
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
        _startDateTime = DateTime.Now;

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

       var endTime = DateTime.Now;

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

        var ts = endTime - _startDateTime;

        _userInteractor.ShowMessageNewLine($"You spent {ts.TotalMinutes} minutes playing.");
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
