using System.Net.Http.Headers;
using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Characters;
using TheFinalBattle.Item;
using TheFinalBattle.Player;


//for (var i = 0; i < 10; i++)
//{
//    Console.WriteLine(GearAttackAction.GetAttackSuccessProbability()); 
//}

//Console.ReadKey();


var trueProgrammer = new HeroCharacter() { Label = "The True Progammer" };
var vinFletcher = new HeroCharacter(15) { Label = "Vin Fletcher" };

var heroSword = new Sword();
var heroDagger = new Dagger();
var bow = new Bow(vinFletcher);



var heroParty = new Party(
        [trueProgrammer, vinFletcher])
{
    Items = [new HealthPotion(), new HealthPotion(), new HealthPotion()]
};
heroParty.AttackGear.Add(heroSword);
heroParty.AttackGear.Add(heroDagger);



var player1 = new Player([heroParty]) { PlayerType = PlayerType.Human };

// battle 1 party setup
var battle1Skeleton1 = new Skeleton();
//var dagger = new Dagger(battle1Skeleton1);
var dagger = new Dagger();
var hammer = new Hammer();



var battle1Party = new Party([battle1Skeleton1])
{
    PartyType = PartyType.Villian,
    Items = [new HealthPotion(), new Scarf(), new Scarf()]
};
battle1Party.AttackGear.Add(dagger);
battle1Party.AttackGear.Add(hammer);


// battle 2 party setup
var battle2Skeleton1 = new Skeleton();
var battle2Skeleton1Dagger = new Dagger();
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
battle2Party.AttackGear.Add(battle2Skeleton1Dagger);
battle2Party.AttackGear.Add(battle2Skeleton2Dagger);


// battle 3 party setup
var battle3Party = new Party([new UncodedOne()]) { PartyType = PartyType.Villian };


var player2 = new Player(
    [
        battle1Party,
        battle2Party,
        battle3Party
    ]);


new BattleRunner(player1, player2, new ConsoleUserInteractor()).Run();




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
        int computerRoundsPlayed = 0;
        var heroLost = false;
        int round = 1;

        foreach (var villainParty in _player2.Parties)
        {
            UseItemAction? computerPlayerPotionAction = default;
            GearEquipAction? computerPlayerGearEquipAction = default;
            GearAttackAction? computerPlayerGearAttackAction = default;

            do
            {
                bool havePotion = false;

                _attackingPlayer = _attackingPlayer == _player1 ? _player2 : _player1;
                var defendingPlayer = _attackingPlayer == _player1 ? _player2 : _player1;



                if (_attackingPlayer.PlayerType == PlayerType.Computer)
                {
                    computerRoundsPlayed++;
                    attackingParty = villainParty;
                }
                else
                {
                    attackingParty = _attackingPlayer.Parties[0];
                }

                defendingParty = defendingPlayer.Parties.Where(x => x.CharacterCount > 0).First();


                var attackingCharacter = attackingParty.PartyType == PartyType.Hero
                        ? attackingParty.GetNextAttackingCharacter()
                        : attackingParty.GetCharacterAt(0);


                var defendingCharacter = defendingParty.PartyType == PartyType.Hero
                        ? defendingParty.GetCurrentAttackingCharacter()
                        : defendingParty.GetCharacterAt(0);
               

                ShowGameStatus(villainParty, round);

                _userInteractor.WriteLine($"It is {attackingCharacter.Name} ({attackingCharacter.Label})'s turn...choose an option");




                var optionCount = 0;
                var attackOptions = attackingCharacter.GetStandardAttackMenuOptions();
                optionCount += attackOptions.Length;


                var gearAttackOption = attackingParty.GetEquippedGearAttackOption(attackingCharacter, optionCount + 1);
                if (gearAttackOption is not null)
                {
                    optionCount++;
                }

                var itemActionsOptions = attackingParty.GetAvailableItemActionsMenu(optionCount + 1);
                optionCount += itemActionsOptions.Count();

                var unequippedGearActionOptions = attackingParty.GetEquippableGearMenu(optionCount + 1);
                optionCount += unequippedGearActionOptions.Count();




                foreach (var option in attackOptions)
                {
                    if (attackingCharacter.StandardAttack == option.Action)
                    {
                        _userInteractor.WriteLine("\nAttacks:\n---------------------------------------");
                    }

                    _userInteractor.Write($"{option.Id} - ");
                    _userInteractor.Write(attackingCharacter.StandardAttack == option.Action ? "" : option.Action.Name);
                    _userInteractor.WriteLine($"{(attackingCharacter.StandardAttack == option.Action ? "Standard Attack (" + attackingCharacter.StandardAttack.Name + ")" : "")} ");
                }


                if (gearAttackOption is not null)
                {
                    _userInteractor.Write($"{gearAttackOption.Id} - ");
                    _userInteractor.WriteLine(gearAttackOption.Action.Name);
                }



                if (attackingParty.Items.Count > 0)
                {
                    _userInteractor.WriteLine("\nAvailable Items:\n---------------------------------------");

                    foreach (var itemActionOption in itemActionsOptions)
                    {
                        if (_attackingPlayer.PlayerType == PlayerType.Computer && !havePotion && computerRoundsPlayed % 4 == 0)
                        {
                            if (itemActionOption.Action is UseItemAction useItemAction)
                            {
                                if (useItemAction.Item is HealthPotion potion)
                                {
                                    havePotion = true;
                                    computerPlayerPotionAction = (UseItemAction)itemActionOption.Action;
                                }
                            }
                        }

                        _userInteractor.WriteLine($"{itemActionOption.Id} - {itemActionOption.Action.Name}");
                    }
                }




                if (unequippedGearActionOptions.Any())
                {
                    _userInteractor.WriteLine("\nAvailable Gear:\n---------------------------------------");

                    foreach (var gearAction in unequippedGearActionOptions)
                    {
                        _userInteractor.WriteLine($"{gearAction.Id} - {gearAction.Action.Name}");

                        if (_attackingPlayer.PlayerType == PlayerType.Computer && computerRoundsPlayed % 2 == 0)
                        {
                            if (attackingCharacter.EquippedGear is null && computerPlayerPotionAction is null)
                            {
                                var gearEquipAction = gearAction.Action as GearEquipAction;

                                if (gearEquipAction!.Gear is Dagger dagger)
                                {
                                    computerPlayerGearEquipAction = gearEquipAction;
                                }
                            }
                        }
                    }
                }



                if (_attackingPlayer.PlayerType == PlayerType.Computer && gearAttackOption is not null)
                {
                    if (attackingCharacter.EquippedGear is not null)
                    {
                        computerPlayerGearAttackAction = gearAttackOption.Action as GearAttackAction;
                    }
                }


                _userInteractor.WriteLine("\nWhat do you want to do?");



                if (computerPlayerPotionAction is not null &&
                        _attackingPlayer.GetHealthRestoreCandidate() is not null)
                {
                    _userInteractor.WriteLine("--------------------------------------", TextColor.Magenta);
                    _userInteractor.WriteLine("Computer auto-selected Health Potion!", TextColor.Magenta);
                    _userInteractor.WriteLine("--------------------------------------", TextColor.Magenta);
                    characterAction = computerPlayerPotionAction!;
                }
                else if (computerPlayerGearAttackAction is not null)
                {
                    characterAction = computerPlayerGearAttackAction;
                }
                else if (computerPlayerGearEquipAction is not null)
                {
                    characterAction = computerPlayerGearEquipAction;
                }
                else if (_attackingPlayer.PlayerType == PlayerType.Computer)
                {
                    characterAction = attackingCharacter.StandardAttack;
                }
                else
                {
                    var humanAttackOptions = attackOptions;

                    humanAttackOptions = humanAttackOptions.Union(itemActionsOptions).ToArray();

                    if (gearAttackOption is not null)
                    {
                        MenuItem[] options = [gearAttackOption];

                        humanAttackOptions = humanAttackOptions.Union(options).ToArray();
                    }

                    humanAttackOptions = humanAttackOptions.Union(unequippedGearActionOptions).ToArray();

                    characterAction = characterAction = GetHumanPlayerAction(humanAttackOptions);
                }


                PerformAction(characterAction, attackingCharacter, defendingCharacter);



                if (defendingCharacter.CurrentHealth == 0)
                {
                    defendingParty.RemoveCharacter(defendingCharacter);

                    _userInteractor.WriteLine($"{defendingCharacter.Name} has been defeated.", TextColor.Red);
                }


                heroLost = _player1.Parties[0].CharacterCount == 0;

                _userInteractor.WriteLine($"\n");

                computerPlayerPotionAction = null;
                computerPlayerGearEquipAction = null;
                computerPlayerGearAttackAction = null;

            }
            while (villainParty.CharacterCount > 0 && !heroLost);

            _userInteractor.WriteLine("\n----------------------------------------", TextColor.Magenta);
            _userInteractor.WriteLine("Enemy Party defeated!!!", TextColor.Magenta);
            _userInteractor.WriteLine("----------------------------------------\n", TextColor.Magenta);

            round++;

            LootVillianItems(villainParty);
            LootVillainGear(villainParty);

        }



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

    private void LootVillainGear(IParty villainParty)
    {
        foreach (var gear in villainParty.GetEquippableGear())
        {
            _userInteractor.WriteLine($"Addded gear: {gear.Name} to Hero party.");
            _player1.Parties[0].AttackGear.Add(gear);
        }

        foreach (var gear in villainParty.GetEquippedGear())
        {
            _userInteractor.WriteLine($"Addded gear: {gear.Name} to Hero party.");
            Gear.UnequipCharacter(gear);
            _player1.Parties[0].AttackGear.Add(gear);
        }

        _userInteractor.WriteLine("");
    }

    private void LootVillianItems(IParty villainParty)
    {
        foreach(var item in villainParty.Items)
        {
            _userInteractor.WriteLine($"Addded Item: {item.Name} to Hero party.");
        }

        _player1.Parties[0].AddItems(villainParty.Items);
        villainParty.Items.Clear();
    }

    private void PerformAction(CharacterAction characterAction, Character attackingCharacter, Character defendingCharacter)
    {
        if (characterAction is GearEquipAction equipAction)
        {
            equipAction.EquipGear(attackingCharacter);
            _userInteractor.WriteLine("--------------------------------------", TextColor.Yellow);
            _userInteractor.WriteLine($"{attackingCharacter.Name} equipped himself with a {equipAction.Gear.Name}", TextColor.Yellow);
            _userInteractor.WriteLine("--------------------------------------", TextColor.Yellow);
        }
        else if (characterAction is GearAttackAction gearAttackAction)
        {
            var damageDealt = gearAttackAction.PerformAttack(defendingCharacter, _userInteractor);
            if (damageDealt == 0)
            {
                _userInteractor.WriteLine($"{attackingCharacter.Name} missed!".ToUpper());
            }
            else
            {
                _userInteractor.WriteLine($"{attackingCharacter.Name} used {characterAction.Name} on {defendingCharacter.Name}", TextColor.Green);
            }
            
        }
        else if (characterAction is StandardAttackAction attackAction)
        {
            var damageDealt = attackAction.PerformAttack(defendingCharacter, _userInteractor);
            if (damageDealt == 0)
            {
                _userInteractor.WriteLine($"{attackingCharacter.Name} missed!".ToUpper());
            }
            else
            {
                _userInteractor.WriteLine($"{attackingCharacter.Name} used {characterAction.Name} on {defendingCharacter.Name}", TextColor.Green);
            }
            
        }
        else if (characterAction is UseItemAction action2)
        {
            action2.UseItem(attackingCharacter);
            _userInteractor.WriteLine($"{attackingCharacter.Name} used {action2.Name}", TextColor.Green);
        }
    }

    private void ShowGameStatus(IParty party, int round)
    {
        _userInteractor.WriteLine($"================================ BATTLE #{round}================================", TextColor.Blue);
        foreach (var hero in _player1.Parties[0].GetCharacters())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0,-10} {1,-20} {2,10}", $"{hero.Name}", $"({hero.Label})", $"(  {hero.CurrentHealth}/{hero.MaximumHP}  )", TextColor.Red);
            Console.ForegroundColor = ConsoleColor.White;
        }
        _userInteractor.WriteLine("----------------------------------- vs ---------------------------------", TextColor.Blue);

        var characters = party.GetCharacters();

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

 

   
}
