using TheFinalBattle.Characters;
using TheFinalBattle.Player;

public class BattleRunner
{
    private IUserInteractor _userInteractor;
    private Player _player1;
    private Player _player2;
 
    public BattleRunner(Player player1, Player player2, IUserInteractor userInteractor)
    {
        _player1 = player1;        
        _player2 = player2; 
        _userInteractor = userInteractor;
    }

    public void Run()
    {
        GameMode gameMode = GetGameMode();

        GetHeroName();

        SetPlayer1Type(gameMode, ref _player1);

        SetPlayer2Type(gameMode, ref _player2);


        new Battle(_player1, _player2, _userInteractor).Play();
    }

    private void SetPlayer2Type(GameMode gameMode, ref Player player)
    {
        player.PlayerType = gameMode switch
        {
            GameMode.Computer_vs_Computer => PlayerType.Computer,
            GameMode.Player_vs_Computer => PlayerType.Computer,
            GameMode.Player_vs_Player => PlayerType.Human,
        };
    }

    private void SetPlayer1Type(GameMode gameMode, ref Player player)
    {
        player.PlayerType = gameMode switch
        {
            GameMode.Computer_vs_Computer => PlayerType.Computer,
            GameMode.Player_vs_Computer => PlayerType.Human,
            GameMode.Player_vs_Player => PlayerType.Human,
        };
    }

    private void GetHeroName()
    {
        foreach (var party in _player1.Parties.Union(_player2.Parties))
        {
            foreach (var character in party.GetCharacters())
            {
                if (character is HeroCharacter)
                {
                    string heroName = "";
                    do
                    {
                        _userInteractor.WriteLine($"Enter the hero's name ({character.Label}):");
                        heroName = _userInteractor.GetUserInput();
                    }
                    while (string.IsNullOrEmpty(heroName));

                    character.Name = heroName;
                }
            }
        }
    }

    private GameMode GetGameMode()
    {
        GameMode mode = GameMode.None;

        _userInteractor.WriteLine("Choose from 1 of 3 game modes:", TextColor.Blue);

        foreach (var value in Enum.GetValues(typeof(GameMode)))
        {
            if ((int)value != 0)
                _userInteractor.WriteLine($"{(int)value} - {value.ToString()}");
        }

        do
        {
            var input = _userInteractor.GetUserInput();
            int selectedValue = 0;

            if (int.TryParse(input, out selectedValue) && Enum.IsDefined(typeof(GameMode), selectedValue))
            {
                mode = (GameMode)selectedValue;
            }
            else
            {
                _userInteractor.WriteLine("Invalid game mode selected. Try again");
            }

        }
        while (mode == GameMode.None);

        return mode;
    }
}
