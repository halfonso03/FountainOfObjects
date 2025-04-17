﻿using TheFinalBattle.Enums;

namespace TheFinalBattle.Actions.AttackActions;

internal class Unravel : AttackAction
{
    public override string Name { get; } = "UNRAVEL";
    public override DamageType DamageType { get; } = DamageType.Decoding;
    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo();

        if (GetAttackSuccessProbability() == 1)
        {
            damageInfo.InflictedDamage = new Random().Next(1, 5);
        }
        else
        {
            damageInfo.AttackMissed = true;
        }

        return damageInfo;
    }
        
        
}
