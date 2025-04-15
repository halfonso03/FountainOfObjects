using TheFinalBattle.Characters;

namespace TheFinalBattle.Actions;

public abstract class AttackAction : CharacterAction
{    
    public override string ToString() => $"{Name.Substring(0, 1)} = {Name}";    
    public string Abbreviation => Name.Substring(0, 1);
    public abstract int DamageDealt();
    public void PerformAttack(Character target, IUserInteractor userInteractor)
    {
        target.DecreaseHealth(DamageDealt());

        userInteractor.WriteLine($"{Name} dealt {DamageDealt()} to {target.Name}");
        userInteractor.WriteLine($"{target.Name} is now at {target.CurrentHealth}/{target.MaximumHP}", TextColor.DarkYellow);
    }
}
