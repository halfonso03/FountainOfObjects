﻿using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Actions.AttackModifiers;

namespace TheFinalBattle.Characters;

public class Skeleton : Character
{    
    public override string Name { get; set; } = "SKELETON";
    public override string Label { get; set; } = string.Empty;
    public override CharacterAction[] Actions { get; init; } = [new NothingAction(), new BoneCrunch()];
    public override CharacterAction StandardAttack => Actions[1];
    public override int InitialHP { get; } = 5;
    public override int MaximumHP { get; } = 5;
    public override IDefenseModifier? DefenseModifier { get; set; } = null;
    public Skeleton()
    {
        CurrentHealth = InitialHP;
    } 
}
