using TheFinalBattle.Actions.AttackModifiers;
using TheFinalBattle.Characters;
using TheFinalBattle.Item;
using TheFinalBattle.Player;

public class BattleRunner
{
    private IUserInteractor _userInteractor;
    private IPlayer _player1;
    private IPlayer _player2;
 
    public BattleRunner(IUserInteractor userInteractor)
    {   
        _userInteractor = userInteractor;
    }


    public void Run()
    {
        GameMode gameMode = GetGameMode();

        _player1 = SetUpHumanPlayer("Player 1");

        GetHerosNames(gameMode, _player1);

        if (gameMode == GameMode.Player_vs_Computer)
        {
            _player2 = SetupComputerPlayer(); 
        }
        else if (gameMode == GameMode.Player_vs_Player)
        {            
            _player2 = SetUpHumanPlayer("Player 2");
            GetHerosNames(gameMode, _player2);
        }
        else
        {
            _player2 = SetupComputerPlayer();
        }

        

        new Battle(_player1, _player2, _userInteractor).Play();
    }

    private IPlayer SetupComputerPlayer()
    {

        //battle 1 party setup
        var battle1Skeleton1 = new Skeleton();
        var battle1Skeleton2 = new Skeleton();
        //var battle1Amarok1 = new StoneAmarok();
        var dagger1 = new Dagger();
        var dagger2 = new Dagger();
        var dagger3 = new Dagger(battle1Skeleton2);
        var hammer = new Hammer();



        var battle1Party = new Party([battle1Skeleton1, battle1Skeleton2])
        {
            PartyType = PartyType.Villian,
            Items = [new HealthPotion()]
        };
        battle1Party.AttackGear.Add(dagger1);
        battle1Party.AttackGear.Add(dagger2);
        


        // battle 2 party setup
        var battle2Skeleton1 = new Skeleton();
        var battle2Skeleton1Dagger = new Dagger(battle2Skeleton1);
        var battle2Skeleton2 = new Skeleton();
        var battle2Skeleton2Dagger = new Dagger();

        var battle2Party = new Party(
            [
                battle2Skeleton1,
                battle2Skeleton2
            ])
        {
            PartyType = PartyType.Villian,
            Items = [new HealthPotion()]
        };



        // battle 3 party setup
        var battle3Party = new Party(
            [new UncodedOne()])
        {
            PartyType = PartyType.Villian,
            Items = [new HealthPotion()]
        };
        //var battle3Party = new Party([new StoneAmarok()]) { PartyType = PartyType.Villian };


        return new Player(
            [
                battle1Party,
                battle2Party,
                battle3Party
            ])
        { PlayerType = PlayerType.Computer };
    }

    private void SetPlayer2Type(GameMode gameMode, ref IPlayer player)
    {
        player.PlayerType = gameMode switch
        {
            GameMode.Computer_vs_Computer => PlayerType.Computer,
            GameMode.Player_vs_Computer => PlayerType.Computer,
            GameMode.Player_vs_Player => PlayerType.Human,
        };
    }

    private void SetPlayer1Type(GameMode gameMode, ref IPlayer player)
    {
        player.PlayerType = gameMode switch
        {
            GameMode.Computer_vs_Computer => PlayerType.Computer,
            GameMode.Player_vs_Computer => PlayerType.Human,
            GameMode.Player_vs_Player => PlayerType.Human,
        };
    }

    private void GetHerosNames(GameMode gameMode, IPlayer player)
    {
        foreach (var party in player.Parties)
        {
            foreach (var character in party.GetCharacters())
            {
                if (character is HeroCharacter)
                {
                    string heroName = "";
                    do
                    {
                        _userInteractor.WriteLine($"Enter the hero's name ({character.Label}) ({player.Name}):");
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


    IPlayer SetUpHumanPlayer(string playerName)
    {
        var trueProgrammer = new HeroCharacter()
        {
            Label = "The True Programmer",
            DefenseModifier = new ObjectSightAttackModifier()
        };
        var vinFletcher = new HeroCharacter(15) { Label = "Vin Fletcher" };

        var heroSword = new BroadSword(trueProgrammer);
        var flamingSword = new FlamingSword();
        var heroDagger1 = new Dagger();
        var heroDagger2 = new Dagger();
        var bow = new Bow(vinFletcher);

        var mylara = new HeroCharacter(12) { Label = "Skorin" };
        //        var cOfc = new CanonOfConsolas(mylara);



        var heroParty = new Party(
                [trueProgrammer])
        {
            Items = [new HealthPotion(), new HealthPotion(), new HealthPotion()],
            
        };
        //heroParty.AttackGear.Add(heroSword);
        heroParty.AttackGear.Add(heroDagger1);
        heroParty.AttackGear.Add(heroDagger2);


        return new Player([heroParty]) { PlayerType = PlayerType.Human, Name = playerName };
    }
}
