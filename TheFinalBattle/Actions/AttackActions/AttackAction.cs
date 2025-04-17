using TheFinalBattle.Characters;
using TheFinalBattle.Enums;


namespace TheFinalBattle.Actions.AttackActions;

public abstract class AttackAction : CharacterAction
{
    public abstract DamageInfo DamageToInflict();
    public virtual DamageType DamageType { get; } = DamageType.Normal;
    public virtual AttackData PerformAttack(Character attacker, Character target)
    {
        DamageInfo damageInfo = DamageToInflict();

        damageInfo.AttackerDamageType = DamageType;
        damageInfo.DefenderDamageTypeModifier = target.AttackModifier?.DefendsDamageType;

        if (target.AttackModifier?.DefendsDamageType == this.DamageType)
        {
            if (!damageInfo.AttackMissed)
            {
                damageInfo.InflictedDamage += target.AttackModifier.ModifyBy;
            }            
        }

        if (damageInfo.InflictedDamage.HasValue)
        {
            target.DecreaseHealthBy(damageInfo.InflictedDamage!.Value);
        }
        

        return new AttackData() { DamageInfo = damageInfo };        
    }

    public static double GetAttackSuccessProbability(int percentChance = 70)
    {
        var random = new Random((int)DateTime.Now.Ticks);
        //var id = random.Next(0, 3);

        //if (id == NO_DAMAGE) return NO_DAMAGE;
        //if (id == FULL_DAMAGE) return FULL_DAMAGE;


        int j = 0;
        for (var i = 1; i < 101; i++)
        {
            if (random.Next(1, 101) >= (100 - percentChance))
            {
                j++;
            }
        }

        return j >= percentChance ? 1 : 0;
    }
}
