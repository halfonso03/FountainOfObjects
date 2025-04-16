namespace TheFinalBattle.Actions;

internal class Unravel : StandardAttackAction
{
    public override string Name { get; } = "UNRAVEL";

    public override int DamageDealt() => new Random().Next(3);
}
