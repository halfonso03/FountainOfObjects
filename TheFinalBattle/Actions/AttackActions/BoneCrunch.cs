using TheFinalBattle.Item;

namespace TheFinalBattle.Actions.AttackActions;

public class BoneCrunch : AttackAction
{
    public override string Name { get; } = "BONE CRUNCH";

    public override int ProbabilityOfSuccess { get; } = 90;

    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo();

        if (GetSuccessProbability(ProbabilityOfSuccess) == 1)
        {
            var random = new Random();
            var damage = random.Next(0, 2);
            damageInfo.InflictedDamage = damage;            
        }
        else
        {
            damageInfo.AttackMissed = true;            
        }

        return damageInfo;
    }
}
