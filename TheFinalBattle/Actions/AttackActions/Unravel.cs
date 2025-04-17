namespace TheFinalBattle.Actions.AttackActions;

internal class Unravel : StandardAttackAction
{
    public override string Name { get; } = "UNRAVEL";

    public override int DamageDealt()
    {
        if (GetAttackSuccessProbability() == 1)
        {
            return (int)Math.Round(new Random().NextDouble() + .5, 0); ;
        }
        else
        {
            return new Random().Next(1, 3); ;
        }
    }
        
        
}
