using TheFinalBattle.Item;

namespace TheFinalBattle.Actions.AttackActions;

public class BoneCrunch : StandardAttackAction
{
    public override string Name { get; } = "BONE CRUNCH";

    public override int DamageDealt()
    {
        //GetAttackSuccessProbability()
        if (1 == 1)
        {
            var damage = (int)Math.Round(new Random().NextDouble() + .9, 0);
            return damage;
        }
        else
        {
            return 0;
        }

    }


    public BoneCrunch() : base()
    {

    }
}
