

namespace TheFinalBattle.Actions.AttackActions
{
    internal class Bite : StandardAttackAction
    {
        public override string Name => "BITE";        

        public override DamageInfo DamageToInflict()
        {
            return new DamageInfo() { InflictedDamage = 1 };
        }
    }
}
