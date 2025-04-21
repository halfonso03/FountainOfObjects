using TheFinalBattle.Item;

namespace TheFinalBattle.Actions.AttackActions;

public class GearAttackAction : AttackAction
{
    public GearAttackAction? AdditionalGearAttackAction { get; set; }

    private readonly Gear _gear;

    public override string Name { get; }

    public GearAttackAction(Gear gear) : base()
    {
        _gear = gear;
        Name = $"{_gear.Name} ({_gear.AttackName})";
    }

    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo() { DamageDealtSource = _gear.DamageDealtSource };

        damageInfo.InflictedDamage = _gear.GetDamageDealt();

        if (damageInfo.InflictedDamage == 0)
        {
            damageInfo.AttackMissed = true;
        }

        //if (_gear.DamageDealtSource == DamageDealtSource.DefaultProbability)
        //{
        //    if (GetSuccessProbability(ProbabilityOfSuccess) == 1)
        //    {
        //        damageInfo.InflictedDamage = _gear.DamageDealt;
        //    }
        //    else
        //    {
        //        damageInfo.AttackMissed = true;
        //    }
        //}
        //else  if (_gear.DamageDealtSource == DamageDealtSource.Custom)
        //{
        //    damageInfo.InflictedDamage = _gear.DamageDealt;

        //    if (damageInfo.InflictedDamage == 0)
        //    {
        //        damageInfo.AttackMissed = true;
        //    }
        //}        
        
        return damageInfo;
    }
}