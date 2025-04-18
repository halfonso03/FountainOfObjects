namespace TheFinalBattle.Actions.AttackActions;

public abstract class StandardAttackAction : AttackAction
{
    protected StandardAttackAction() { }

    public override DamageInfo DamageToInflict()
    {
        var damageInfo = new DamageInfo(); 
        
        if (GetSuccessProbability(ProbabilityOfSuccess) == 1)
        {
            damageInfo.InflictedDamage = 1;
        }
        else
        {            
            damageInfo.AttackMissed = true;            
        }

        return damageInfo;
    }
}
