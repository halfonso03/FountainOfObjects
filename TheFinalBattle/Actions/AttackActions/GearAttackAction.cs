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
        var damageInfo = new DamageInfo() { DamageDealtSource = _gear.DamageDealtSource };

        if (_gear.DamageDealtSource == DamageDealtSource.StandardProbabilityCalculation)
        {
            if (GetSuccessProbability(ProbabilityOfSuccess) == 1)
            {
                damageInfo.InflictedDamage = _gear.DamageDealt;
            }
            else
            {
                damageInfo.AttackMissed = true;
            }
        }
        else  if (_gear.DamageDealtSource == DamageDealtSource.Custom)
        {
            damageInfo.InflictedDamage = _gear.DamageDealt;

            if (damageInfo.InflictedDamage == 0)
            {
                damageInfo.AttackMissed = true;
            }
        }        
        
        return damageInfo;
    }
}