namespace TheFinalBattle.Actions.AttackActions;

public abstract class StandardAttackAction : AttackAction
{
    private const double NO_DAMAGE = 0;
    private const double FULL_DAMAGE = 1;

    protected StandardAttackAction() { }

    public static double GetAttackSuccessProbability()
    {
        var random = new Random((int)DateTime.Now.Ticks);
        double[] probabilites = [0, .5, 1];
        var randomPosition = random.Next(0, 3);

        if (randomPosition == 0)
            return NO_DAMAGE;
        else if (randomPosition == 2)
            return FULL_DAMAGE;
        else
        {
            if (random.Next(0, 2) == 0)
                return NO_DAMAGE;
            else
                return FULL_DAMAGE;
        }
    }
}
