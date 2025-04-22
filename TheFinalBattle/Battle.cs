using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Characters;
using TheFinalBattle.Item;
using TheFinalBattle.Player;

public class Battle
{   
    private IUserInteractor _userInteractor;
    private IPlayer _player1;
    private IPlayer _player2;
    private IPlayer _attackingPlayer;
    private IPlayer _defendingPlayer;

    public Battle(IPlayer player1, IPlayer player2, IUserInteractor userInteractor)
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

        foreach (var enemyParty in _player2.Parties)
        {
            UseItemAction? computerPlayerPotionAction = default;
            GearEquipAction? computerPlayerGearEquipAction = default;
            GearAttackAction? computerPlayerGearAttackAction = default;

            Character? defendingCharacter = null;

            do
            {
                bool willUsePotion = false;
                var optionCount = 0;

                _attackingPlayer = _attackingPlayer == _player1 ? _player2 : _player1;
                _defendingPlayer = _attackingPlayer == _player1 ? _player2 : _player1;

                

                if (_attackingPlayer.PlayerType == PlayerType.Computer)
                {
                    computerRoundsPlayed++;
                    attackingParty = enemyParty;
                }
                else
                {
                    attackingParty = _attackingPlayer.Parties[0];
                }

                defendingParty = _defendingPlayer.Parties.Where(x => x.CharacterCount > 0).First();


                var attackingCharacter = attackingParty.PartyType == PartyType.Hero
                        ? attackingParty.GetNextAttackingCharacter()
                        : attackingParty.GetHealthiestCharacter();

                
                


                if (defendingCharacter is null)
                {
                    defendingCharacter = defendingParty.GetCharacterAt(0);
                }
                else
                {
                    if (_defendingPlayer.PlayerType == PlayerType.Computer)
                    {
                        var healthiest = defendingParty.GetHealthiestCharacter();
                        if (defendingCharacter.CurrentHealth < healthiest.CurrentHealth)
                        {
                            defendingCharacter = healthiest;
                        }
                    }                    
                }


                ShowGameStatus(enemyParty, round, attackingCharacter, defendingCharacter);




                optionCount = GetActions(attackingParty, 
                    optionCount, 
                    attackingCharacter, 
                    out MenuItem[] attackOptions, 
                    out MenuItem? gearAttackOption, 
                    out IEnumerable<MenuItem> itemActionsOptions, 
                    out IEnumerable<MenuItem> unequippedGearActionOptions);


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
                    WriteGearAttackOptions(gearAttackOption);
                }



                if (attackingParty.Items.Count > 0)
                {
                    _userInteractor.WriteLine("\nAvailable Items:\n---------------------------------------");

                    foreach (var itemActionOption in itemActionsOptions)
                    {
                        if (_attackingPlayer.PlayerType == PlayerType.Computer && !willUsePotion
                            && attackingCharacter.CurrentHealth <= attackingCharacter.MaximumHP * .2) // removed logic for every fourth turn
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
                    _userInteractor.WriteLine("\n--------------------------------------", TextColor.Magenta);
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

                Thread.Sleep(200);
                _userInteractor.WriteLine("**************** ATTACK RESULT ****************", TextColor.Red);
                ShowGameStatus(enemyParty, round, attackingCharacter, defendingCharacter);
                _userInteractor.WriteLine("***********************************************\n\n", TextColor.Red);
                Thread.Sleep(400);

                defendingCharacter = attackingCharacter;

            

                if (attackingParty.PartyType == PartyType.Villian)
                {
                    _userInteractor.WriteLine("Press enter to continue...", TextColor.White);                    
                    Console.ReadLine();
                }

            }
            while (enemyParty.CharacterCount > 0 && !heroLost);


            
            _userInteractor.WriteLine("\n----------------------------------------", TextColor.Magenta);
            _userInteractor.WriteLine("Enemy Party defeated!!!", TextColor.Magenta);
            _userInteractor.WriteLine("----------------------------------------\n", TextColor.Magenta);

            round++;

            LootVillainItems(enemyParty);
            LootVillainGear(enemyParty);
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

    private static int GetActions(IParty attackingParty, int optionCount, Character attackingCharacter, out MenuItem[] attackOptions, out MenuItem? gearAttackOption, out IEnumerable<MenuItem> itemActionsOptions, out IEnumerable<MenuItem> unequippedGearActionOptions)
    {
        attackOptions = attackingCharacter.GetStandardAttackMenuOptions();
        optionCount += attackOptions.Length;


        gearAttackOption = attackingParty.GetEquippedGearAttackOption(attackingCharacter, optionCount + 1);
        if (gearAttackOption is not null)
        {
            optionCount++;
        }

        itemActionsOptions = attackingParty.GetAvailableItemActionsMenu(optionCount + 1);
        optionCount += itemActionsOptions.Count();

        unequippedGearActionOptions = attackingParty.GetEquippableGearMenu(optionCount + 1);
        optionCount += unequippedGearActionOptions.Count();
        return optionCount;
    }

    void WriteGearAttackOptions(MenuItem m)
    {
        
        int x = 0;
        var gearA = (GearAttackAction)m.Action;

        WriteGearAttackOption(gearA, ref x);

        if (x == 1)
        {
            _userInteractor.WriteLine($"{m.Id} - {gearA.Name}"); ;
        }
        else
        {
            _userInteractor.WriteLine($"{m.Id} - {gearA.Name} x {x}"); ;
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
            Gear.UnequipFromCharacter(gear);
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
                
                if (_defendingPlayer.PlayerType == PlayerType.Computer)
                {
                    _userInteractor.WriteLine($"**** {defendingCharacter.Name} is now at {defendingCharacter.CurrentHealth}/{defendingCharacter.MaximumHP}", TextColor.DarkYellow);
                }
                else
                {
                    _userInteractor.WriteLine($"**** {defendingCharacter.Name} ({defendingCharacter.Label}) is now at {defendingCharacter.CurrentHealth}/{defendingCharacter.MaximumHP}", TextColor.DarkYellow);
                }

                
            }
            
        }
        else if (characterAction is UseItemAction useItemAction)
        {
            useItemAction.UseItem(attackingCharacter);
            _userInteractor.WriteLine($"{attackingCharacter.Name} used {useItemAction.Name}", TextColor.Green);
        }
    }

    

    private void ShowGameStatus(IParty enemyParty, int round, Character attackingCharacter, Character defendingCharacter)
    {
        _userInteractor.WriteLine($"==================================== BATTLE #{round}====================================", TextColor.Blue);
        foreach (var hero in _player1.Parties[0].GetCharacters())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0,-10} {1,-20} {2,10}", $"{hero.Name}", $"({hero.Label})", $"(  {hero.CurrentHealth}/{hero.MaximumHP}  )", TextColor.Red);
            Console.ForegroundColor = ConsoleColor.White;
        }
        _userInteractor.WriteLine("---------------------------------------- vs --------------------------------------", TextColor.Blue);

        Console.ForegroundColor = ConsoleColor.Red;

        if (_player2.Parties.Contains(attackingCharacter.Party))
        {
            if (attackingCharacter.EquippedGearItems.Count() > 0)
            {
                Console.Write("{0,0} {1,70}", "", $"{attackingCharacter.Id} - {attackingCharacter.Name} (  {attackingCharacter.CurrentHealth}/{attackingCharacter.MaximumHP}  )");
                var gear = string.Join(",", attackingCharacter.EquippedGearItems.Select(x => x.Name).ToArray());
                Console.WriteLine($"({gear}) attacker");
            }
            else
            {
                Console.WriteLine("{0,0} {1,80}", "", $"{attackingCharacter.Id} - {attackingCharacter.Name} (  {attackingCharacter.CurrentHealth}/{attackingCharacter.MaximumHP} ) attacker");
            }
        }


        if (_player2.Parties.Contains(defendingCharacter.Party))
        {
            if (defendingCharacter.EquippedGearItems.Count() > 0)
            {
                Console.Write("{0,0} {1,82}", "", $"{defendingCharacter.Id} - {defendingCharacter.Name} (  {defendingCharacter.CurrentHealth}/{defendingCharacter.MaximumHP}  )");
                var gear = string.Join(",", defendingCharacter.EquippedGearItems.Select(x => x.Name).ToArray());
                Console.WriteLine($"({gear}) target");
            }
            else
            {
                Console.WriteLine("{0,0} {1,80}", "", $"{defendingCharacter.Id} - {defendingCharacter.Name} (  {defendingCharacter.CurrentHealth}/{defendingCharacter.MaximumHP} ) target");
            }
        }

        foreach (var character in enemyParty.GetCharacters())
        {
            if (attackingCharacter != character && defendingCharacter != character)
            {
                if (character.EquippedGearItems.Count() > 0)
                {
                    Console.Write("{0,0} {1,70}", "", $"{character.Id} - {character.Name} (  {character.CurrentHealth}/{character.MaximumHP}  )");
                    var gear = string.Join(",", character.EquippedGearItems.Select(x => x.Name).ToArray());
                    Console.WriteLine($"({gear})");
                }
                else
                {
                    Console.WriteLine("{0,0} {1,70}", "", $"{character.Id} - {character.Name} (  {character.CurrentHealth}/{character.MaximumHP}  )");
                }
            }
        }
        _userInteractor.WriteLine("==================================================================================", TextColor.Blue);
        _userInteractor.WriteLine($"It is {attackingCharacter.Name} ({(attackingCharacter.Label != string.Empty ? $"({attackingCharacter.Label})" : "")})'s turn...choose an option", TextColor.White);
        _userInteractor.WriteLine("==================================================================================", TextColor.Blue);
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
