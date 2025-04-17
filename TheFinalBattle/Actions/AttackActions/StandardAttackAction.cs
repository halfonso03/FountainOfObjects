namespace TheFinalBattle.Actions.AttackActions;

public abstract class StandardAttackAction : AttackAction
{
    private const double NO_DAMAGE = 0;
    private const double FULL_DAMAGE = 1;

    protected StandardAttackAction() { }

    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo(); 
        
        if (GetAttackSuccessProbability() == 1)
        {
            damageInfo.InflictedDamage = 1;
        }
        else
        {            
            damageInfo.AttackMissed = true;            
        }

        return damageInfo;
    }

    //public static double GetAttackSuccessProbability(int percentChance = 70)
    //{
    //    var random = new Random((int)DateTime.Now.Ticks);        
    //    //var id = random.Next(0, 3);

    //    //if (id == NO_DAMAGE) return NO_DAMAGE;
    //    //if (id == FULL_DAMAGE) return FULL_DAMAGE;


    //    int j = 0;
    //    for (var i = 1; i < 101; i++)
    //    {
    //        if (random.Next(1, 101) >= (100 - percentChance))
    //        {
    //            j++;
    //        }
    //    }

    //    return j >= percentChance ? 1 : 0;                    
    //}
}
