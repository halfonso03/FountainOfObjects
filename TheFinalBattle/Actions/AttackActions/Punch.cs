﻿namespace TheFinalBattle.Actions.AttackActions;
public class Punch : StandardAttackAction
{
    public override string Name { get; } = "PUNCH";
    public override int ProbabilityOfSuccess { get; } = 100;
}
