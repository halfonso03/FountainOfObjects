using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Characters;
using TheFinalBattle.Item;
using TheFinalBattle.Player;

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
                bool willUsePotion = false;

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
                _userInteractor.WriteLine("========================================================================", TextColor.Blue);



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

                    wrapper(gearAttackOption);

                    
                }



                if (attackingParty.Items.Count > 0)
                {
                    _userInteractor.WriteLine("\nAvailable Items:\n---------------------------------------");

                    foreach (var itemActionOption in itemActionsOptions)
                    {
                        if (_attackingPlayer.PlayerType == PlayerType.Computer && !willUsePotion
                            && attackingCharacter.CurrentHealth <= attackingCharacter.MaximumHP * .2 ) // removed logic for every fourth turn
                        {
                            if (itemActionOption.Action is UseItemAction useItemAction)
                            {
                                if (useItemAction.Item is HealthPotion potion)
                                {
                                    willUsePotion = true;
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

                    foreach (var gearActionMenuOption in unequippedGearActionOptions)
                    {
                        _userInteractor.WriteLine($"{gearActionMenuOption.Id} - {gearActionMenuOption.Action.Name}");

                        if (_attackingPlayer.PlayerType == PlayerType.Computer && !willUsePotion) // && computerRoundsPlayed % 2 == 0
                        {
                            if (attackingCharacter.EquippedGearItems.Count() == 0 && computerPlayerPotionAction is null)
                            {
                                var gearEquipAction = gearActionMenuOption.Action as GearEquipAction;

                                computerPlayerGearEquipAction = gearEquipAction;

                                var gear = gearEquipAction!.Gear;

                                if (gearEquipAction!.Gear.Pairable)
                                {
                                    var pairingGearEquipAction = unequippedGearActionOptions
                                            .Where(x => x.Action != gearActionMenuOption.Action)
                                            .Select(x => (GearEquipAction)x.Action)
                                            .Where(x => x.Gear.Pairable 
                                                    && !x.Gear.Equipped 
                                                    && x.Gear != gear
                                                    && x.Gear.GetType() == gear.GetType())
                                            .FirstOrDefault();

                                    if (pairingGearEquipAction is not null)
                                    {
                                        computerPlayerGearEquipAction!.AdditionalGearEquipAction = pairingGearEquipAction;
                                        gear.PairedGear = pairingGearEquipAction.Gear;
                                        pairingGearEquipAction.Gear.PairedGear = gear;
                                        break;
                                    }

                                }
                            }
                        }
                    }
                }



                if (_attackingPlayer.PlayerType == PlayerType.Computer && gearAttackOption is not null)
                {
                    //if (attackingCharacter.EquippedGear is not null)
                    if (attackingCharacter.EquippedGearItems.Count() > 0)
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
                    _userInteractor.WriteLine("--------------------------------------\n", TextColor.Magenta);
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

                if (attackingParty.PartyType == PartyType.Villian)
                {
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                }

            }
            while (villainParty.CharacterCount > 0 && !heroLost);

            _userInteractor.WriteLine("\n----------------------------------------", TextColor.Magenta);
            _userInteractor.WriteLine("Enemy Party defeated!!!", TextColor.Magenta);
            _userInteractor.WriteLine("----------------------------------------\n", TextColor.Magenta);

            round++;

            LootVillainItems(villainParty);
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


    void wrapper(MenuItem m)
    {
        
        int x = 0;
        var gearA = m.Action as GearAttackAction;
        WriteGearAttackOption(gearA!, ref x);

        if (x == 1)
        {
            _userInteractor.WriteLine($"{m.Id} - {gearA!.Name}"); ;
        }
        else
        {
            _userInteractor.WriteLine($"{m.Id} - {gearA!.Name} x {x}"); ;
        }
        
    }

    void WriteGearAttackOption(GearAttackAction? ga, ref int y)
    {
        if (ga is not null)
        {
            y++;

            WriteGearAttackOption(ga.AdditionalGearAttackAction!, ref y);
        }
        
    }

    

    private void LootVillainGear(IParty villainParty)
    {
        foreach (var gear in villainParty.GetEquippableGear())
        {
            _userInteractor.WriteLine($"Added gear: {gear.Name} to Hero party.");
            _player1.Parties[0].AttackGear.Add(gear);
        }

        foreach (var gear in villainParty.GetEquippedGear())
        {
            _userInteractor.WriteLine($"Added gear: {gear.Name} to Hero party.");
            Gear.UnequipCharacter(gear);
            _player1.Parties[0].AttackGear.Add(gear);
        }

        _userInteractor.WriteLine("");
    }

    private void LootVillainItems(IParty villainParty)
    {
        foreach(var item in villainParty.Items)
        {
            _userInteractor.WriteLine($"Added Item: {item.Name} to Hero party.");
        }

        _player1.Parties[0].AddItems(villainParty.Items);
        villainParty.Items.Clear();
    }

    private void PerformAction(CharacterAction characterAction, Character attackingCharacter, Character defendingCharacter)
    {
        if (characterAction is GearEquipAction equipAction)
        {
            foreach (string key in GearEquipAction.CompiledGear.Keys)
                GearEquipAction.CompiledGear[key] = 0;                


            var actionResult = equipAction.EquipGear(attackingCharacter);

            GearEquipAction.GearActionCompile(actionResult);           

            _userInteractor.WriteLine("---------------------------------------------", TextColor.Yellow);

            foreach (KeyValuePair<string, int> kvp in GearEquipAction.CompiledGear)
            {
                if (kvp.Value > 1)
                {
                    _userInteractor.WriteLine($"{attackingCharacter.Name} equipped himself with {kvp.Value} {kvp.Key}'s", TextColor.Yellow);
                }
                else if (kvp.Value == 1)
                {
                    _userInteractor.WriteLine($"{attackingCharacter.Name} equipped himself with a {kvp.Key}", TextColor.Yellow);
                }
            }

            _userInteractor.WriteLine("---------------------------------------------", TextColor.Yellow);
        }
        else if (characterAction is AttackAction attackAction)
        {
            var attackData = attackAction.PerformAttack(attackingCharacter, defendingCharacter);

            if (attackAction is GearAttackAction gearAttackAction)
            {
                if (gearAttackAction.AdditionalGearAttackAction is not null)
                {
                    var ad2 = gearAttackAction.AdditionalGearAttackAction.PerformAttack(attackingCharacter, defendingCharacter);

                    attackData.DamageInfo.InflictedDamage += ad2.DamageInfo.InflictedDamage;
                    attackData.DamageInfo.StrikeCount++;
                }
            }

            _userInteractor.WriteLine($"* {attackingCharacter.Name} used {characterAction.Name} x {attackData.DamageInfo.StrikeCount} on {defendingCharacter.Name}", TextColor.Green);

            if (!attackData.DamageInfo.AttackMissed && attackData.DamageInfo.InflictedDamage == 0)
            {
                _userInteractor.WriteLine($"** {attackingCharacter.Name} didn't miss, but dealt 0 damage on {defendingCharacter.Name}!".ToUpper());
            }
            else if (attackData.DamageInfo.AttackMissed)
            {
                _userInteractor.WriteLine($"** {attackingCharacter.Name} missed!".ToUpper());
            }            
            else
            {
                if (defendingCharacter.DefenseModifier is not null && attackData.DamageInfo.DefenseModifierUsed)
                {
                    _userInteractor.WriteLine($"** Attack modifier for {defendingCharacter.Name} decreased damage taken by {Math.Abs(defendingCharacter.DefenseModifier.ModifyBy)} ** ");
                }
                
                _userInteractor.WriteLine($"*** {attackingCharacter.Name} dealt {attackData.DamageInfo.InflictedDamage} to {defendingCharacter.Name}");
                _userInteractor.WriteLine($"**** {defendingCharacter.Name} ({defendingCharacter.Label}) is now at {defendingCharacter.CurrentHealth}/{defendingCharacter.MaximumHP}", TextColor.DarkYellow);
            }
            
        }
        else if (characterAction is UseItemAction useItemAction)
        {
            useItemAction.UseItem(attackingCharacter);
            _userInteractor.WriteLine($"{attackingCharacter.Name} used {useItemAction.Name}", TextColor.Green);
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

        _userInteractor.WriteLine("Gear:");

        foreach (var g in party.AttackGear)
        {
            _userInteractor.WriteLine($"{g.Name} Equipped:{g.Equipped} by {(g.EquippedCharacter == null ? "none" : g.EquippedCharacter!.Name)}");
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
