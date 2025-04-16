
namespace TheFinalBattle.Actions;

public class BoneCrunch : StandardAttackAction
{
    public override string Name { get; } = "BONE CRUNCH";
    public override int DamageDealt() => (int)Math.Round(new Random().NextDouble() + .2, 0);
}
