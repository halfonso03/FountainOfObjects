using TheFinalBattle.Enums;

namespace TheFinalBattle.Actions.AttackActions;

internal class Unravel : AttackAction
{
    public override string Name { get; } = "UNRAVEL";
    public override DamageType DamageTypeInflicted { get; } = DamageType.Decoding;
    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo() { AttackerDamageType = DamageTypeInflicted };

        if (GetSuccessProbability(96) == 1)
        {
            damageInfo.InflictedDamage = new Random().Next(2, 7);
        }
        else
        {
            damageInfo.AttackMissed = true;
        }

        return damageInfo;
    }
        
        
}
