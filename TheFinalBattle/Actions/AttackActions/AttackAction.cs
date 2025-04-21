using TheFinalBattle.Characters;
using TheFinalBattle.Enums;


namespace TheFinalBattle.Actions.AttackActions;

public abstract class AttackAction : CharacterAction
{
    public virtual int ProbabilityOfSuccess { get; } = 90;
    public abstract DamageInfo DamageToInflict();
    public virtual DamageType DamageTypeInflicted { get; } = DamageType.Normal;
    public virtual AttackData PerformAttack(Character attacker, Character target)
    {
        DamageInfo damageInfo = DamageToInflict();

        damageInfo.AttackerDamageType = DamageTypeInflicted;
        damageInfo.DefenderDamageType = target.DefenseModifier?.DefendsFromDamageType;

        if (target.DefenseModifier?.DefendsFromDamageType == this.DamageTypeInflicted)
        {
            if (!damageInfo.AttackMissed && damageInfo.InflictedDamage > 0)
            {
                damageInfo.InflictedDamage += target.DefenseModifier!.ModifyBy;
                if (damageInfo.InflictedDamage < 0) damageInfo.InflictedDamage = 0;
            }
            damageInfo.DefenseModifierUsed = true;
        }
       

        if (damageInfo.InflictedDamage.HasValue && damageInfo.InflictedDamage.Value != 0)
        {
            var newHealth = target.GetProposedNewHealth(damageInfo.InflictedDamage!.Value);

            if (newHealth > target.InitialHP)
            {
                target.RestoreFullHealth();
            }
            else
            {
                target.SetHealth(newHealth);                
            }

        }
        

        return new AttackData() { DamageInfo = damageInfo };        
    }


    public static Random RandomGen = new Random(DateTime.Now.Millisecond);
    public static double GetSuccessProbability(int percentChance = 70)
    {
        int success = 0;
        int randomValueBetween0And99 = RandomGen.Next(100);
        if (randomValueBetween0And99 > (100 - percentChance))
        {
            success = 1;
        }

        return success;
    }

    public static double _GetSuccessProbabilityNOTUSED(int percentChance)
    {
        var random = new Random();

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
