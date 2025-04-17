using TheFinalBattle.Actions.AttackActions;

namespace TheFinalBattle.Actions;

public class NothingAction : StandardAttackAction
{    
    public override string Name { get; } = "NOTHING";    
    public override DamageInfo DamageToInflict() => new DamageInfo { InflictedDamage = 0 };
}
