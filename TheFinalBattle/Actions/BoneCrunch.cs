
namespace TheFinalBattle.Actions;

public class BoneCrunch : AttackAction
{
    public override string Name { get; set; } = "BONE CRUNCH";
    public override int DamageDealt() => (int)Math.Round(new Random().NextDouble() + .2, 0);
}
