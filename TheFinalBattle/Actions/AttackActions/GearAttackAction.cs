using TheFinalBattle.Item;

namespace TheFinalBattle.Actions.AttackActions;

public class GearAttackAction : AttackAction
{
    private readonly Gear _gear;

    public override string Name { get; }

    public GearAttackAction(Gear gear) : base()
    {
        _gear = gear;
        Name = $"{_gear.Name} ({_gear.AttackName})";
    }

    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo();

        if (GetAttackSuccessProbability() == 1)
        {
            damageInfo.InflictedDamage = _gear.DamageDealt;
        }
        else
        {
            damageInfo.AttackMissed = true;
        }

        return damageInfo;
    }
}