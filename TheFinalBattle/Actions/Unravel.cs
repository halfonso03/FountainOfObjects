namespace TheFinalBattle.Actions;

internal class Unravel : AttackAction
{
    public override string Name { get; set; } = "UNRAVEL";

    public override int DamageDealt() => new Random().Next(3);
}
