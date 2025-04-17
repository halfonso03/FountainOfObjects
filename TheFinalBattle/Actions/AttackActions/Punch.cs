namespace TheFinalBattle.Actions.AttackActions;
public class Punch : StandardAttackAction
{
    public override string Name { get; } = "PUNCH";
    public override int DamageDealt()
    {
        if (GetAttackSuccessProbability() == 1)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }



    public Punch() : base()
    {

    }
}
