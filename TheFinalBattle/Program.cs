using TheFinalBattle.Actions;
using TheFinalBattle.Characters;
using TheFinalBattle.Player;

var party1 = new Party(
        [new HeroCharacter()]);
party1.Items = [new HealthPotion(), new HealthPotion(), new HealthPotion()];



var player1 = new Player([party1]) { PlayerType = PlayerType.Human };

var party2 = new Party(1) { PartyType = PartyType.Villian };
party2.Items = [new HealthPotion()];

var party3 = new Party(2) { PartyType = PartyType.Villian };
party3.Items = [new HealthPotion()];

var party4 = new Party([new UncodedOne()]) { PartyType = PartyType.Villian };

var player2 = new Player(
    [
        party2, 
        party3,
        party4        
    ]);



new Battle(
        player1, 
        player2,
        new ConsoleUserInteractor())
    .Play();



public class Battle
{   
    private IUserInteractor _userInteractor;
    private Player _player1;
    private Player _player2;
    private Player _attackingPlayer;

    public Battle(Player player1, Player player2, IUserInteractor userInteractor)
    {
        _player1 = player1;
        _player2 = player2;        
        _attackingPlayer = player2;        
        _userInteractor = userInteractor;
    }

    public void Play()
    {

        IParty attackingParty, defendingParty;
        CharacterAction characterAction;

        var heroLost = false;

        GameMode gameMode = GetGameMode();

        SetupGame();
        
        SetPlayer1Type(gameMode, ref _player1);

        SetPlayer2Type(gameMode, ref _player2);
        


        do
        {
            _attackingPlayer = _attackingPlayer == _player1 ? _player2 : _player1;
            var defendingPlayer = _attackingPlayer == _player1 ? _player2 : _player1;

            if (_attackingPlayer.PlayerType == PlayerType.Computer)
            {
                attackingParty = _attackingPlayer.Parties.Where(x => x.CharacterCount > 0).First();                
            }
            else
            {
                attackingParty = _attackingPlayer.Parties[0];
            }

            defendingParty = defendingPlayer.Parties.Where(x => x.CharacterCount > 0).First();

            var attackingCharacter = attackingParty.GetCharacterAt(0);
            var defendingCharacter = defendingParty.GetCharacterAt(0);


            ShowGameStatus();

            _userInteractor.WriteLine($"It is {attackingCharacter.Name}'s turn... {attackingCharacter.Guid}");





            var attackOptions = attackingCharacter.GetAttackMenu();
            
            foreach (var option in attackOptions)
            {
                _userInteractor.Write($"{option.Id} - ");
                _userInteractor.Write(attackingCharacter.StandardAttack == option.Action ? "" : option.Action.Name);
                _userInteractor.WriteLine($"{(attackingCharacter.StandardAttack == option.Action ? "Standard Attack (" + attackingCharacter.StandardAttack.Name + ")" : "")} ");
            }


            var itemsOptions = attackingParty.GetItemsMenu(attackOptions.Length + 1);

            if (attackingParty.Items.Count > 0)
            {               
                foreach (var item in itemsOptions)
                {
                    _userInteractor.WriteLine($"{item.Id} - {item.Action.Name}");
                }                                   
            }



            _userInteractor.WriteLine("What do you want to do?");

            characterAction = (_attackingPlayer.PlayerType == PlayerType.Human)
                ? GetHumanPlayerAction(attackOptions.Union(itemsOptions).ToArray())
                : attackingCharacter.StandardAttack;            


            if (characterAction is AttackAction action)
            {
                action.PerformAttack(defendingCharacter, _userInteractor);
                _userInteractor.WriteLine($"{attackingCharacter.Name} used {characterAction.Name} on {defendingCharacter.Name}", TextColor.Green);
            }
            else if (characterAction is UseItemAction action2)
            {
                action2.UseItem(attackingCharacter);

                _userInteractor.WriteLine($"{attackingCharacter.Name} used {action2.Name}", TextColor.Green);
            }
            

            if (defendingCharacter.CurrentHealth == 0)
            {
                defendingParty.RemoveCharacter(defendingCharacter);

                _userInteractor.WriteLine($"{defendingCharacter.Name} has been defeated.", TextColor.Red);
            }


            heroLost = _player1.Parties[0].CharacterCount == 0;

            _userInteractor.WriteLine($"\n");
            Thread.Sleep(500);
        }
        while (_player2.Parties.Where(x => x.CharacterCount > 0).Any() && !heroLost);


        _userInteractor.WriteLine("Game over");


        if (heroLost)
        {
            _userInteractor.WriteLine("The villains have won");
        }
        else
        {
            _userInteractor.WriteLine("The heroes have won");
        }



    }

    private void ShowGameStatus()
    {
        _userInteractor.WriteLine("================================ BATTLE ================================", TextColor.Blue);
        var hero = _player1.Parties[0].GetCharacterAt(0);
        _userInteractor.WriteLine($"{hero.Name}                         (  {hero.CurrentHealth}/{hero.MaximumHP}  )", TextColor.Red);
        _userInteractor.WriteLine("----------------------------------- vs ---------------------------------", TextColor.Blue);
        


        var characters = _player2.Parties.SelectMany(x => x.GetCharacters());

        foreach(var character in characters)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0,0} {1,70}", "", $"{character.Name} (  {character.CurrentHealth}/{character.MaximumHP}  )");
            Console.ForegroundColor = ConsoleColor.White;
        }
        _userInteractor.WriteLine("========================================================================", TextColor.Blue);

    }

    private CharacterAction GetHumanPlayerAction(MenuItem[] attackOptions)
    {
        CharacterAction attack;
        int selectedOption = 0;

        do
        {
            var input = _userInteractor.GetUserInput();
            if (!int.TryParse(input, out selectedOption))
            {
                _userInteractor.WriteLine("Invalid action selected. Try again.");
            }
            else if (!attackOptions.Any(x => x.Id == selectedOption))
            {
                _userInteractor.WriteLine("Invalid action selected. Try again.");
            }
        }
        while (selectedOption == 0 || selectedOption > attackOptions.Length);

        attack = attackOptions.First(x => x.Id == selectedOption).Action;
        return attack;
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

    private void SetupGame()
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
                        _userInteractor.WriteLine("Enter the hero's name:");
                        heroName = _userInteractor.GetUserInput();
                    }
                    while (string.IsNullOrEmpty(heroName));

                    party.GetCharacterAt(0).Name = heroName;
                }                
            }
        }
    }
}


public interface IItem
{
    string Name { get; }
    void UseItem(Character character);
    bool Used { get; }
}

public class HealthPotion : IItem
{
    private bool _used = false;
    private readonly int HEALTH_INCREASE = 10;
    
    public string Name { get; } = "Health Potion";
    
    public bool Used { get { return _used; } }
    
    public void UseItem(Character character)
    {
        character.IncreaseHealthBy(HEALTH_INCREASE);
        _used = true;
    }
}