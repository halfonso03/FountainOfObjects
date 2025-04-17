using TheFinalBattle.Characters;

namespace TheFinalBattle.Actions.AttackActions;

public abstract class AttackAction : CharacterAction
{
    public abstract int DamageDealt();

    public int PerformAttack(Character target, IUserInteractor userInteractor)
    {
        var damage = DamageDealt();

        target.DecreaseHealthBy(damage);

        userInteractor.WriteLine($"{Name} dealt {damage} to {target.Name}");
        userInteractor.WriteLine($"{target.Name} {target.Label} is now at {target.CurrentHealth}/{target.MaximumHP}", TextColor.DarkYellow);

        return damage;
    }
}
