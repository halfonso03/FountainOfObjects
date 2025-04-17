

namespace TheFinalBattle.Actions.AttackActions
{
    internal class Bite : AttackAction
    {
        public override string Name => "BITE";

        public override DamageInfo DamageToInflict()
        {
            return new DamageInfo() { InflictedDamage = 1 };
        }
    }
}
