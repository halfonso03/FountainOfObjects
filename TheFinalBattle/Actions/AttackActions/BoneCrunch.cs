using TheFinalBattle.Item;

namespace TheFinalBattle.Actions.AttackActions;

public class BoneCrunch : AttackAction
{
    public override string Name { get; } = "BONE CRUNCH";

    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo();

        if (GetAttackSuccessProbability() == 1)
        {
            var damage = (int)Math.Round(new Random().NextDouble() + .9, 0);
            damageInfo.InflictedDamage = damage;            
        }
        else
        {
            damageInfo.AttackMissed = true;            
        }

        return damageInfo;
    }
}
