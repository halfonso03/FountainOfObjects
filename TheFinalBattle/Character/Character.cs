
using TheFinalBattle.Actions;

namespace TheFinalBattle.Characters;

public abstract class Character
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public abstract CharacterAction[] Actions { get; init; }
    public abstract string Name { get; set; }
    public int HitPoints { get; set; } = 100;
    public abstract CharacterAction StandardAttack { get; }
    public Party Party { get; internal set; }
    public abstract int InitialHP { get; }
    public abstract int MaximumHP { get; }
    //public abstract void DecreaseHealth(int damage);
    protected internal int CurrentHealth { get; set; }
    public MenuItem[] GetAttackMenu()
    {
        var attackActions = Actions
            .OfType<AttackAction>()
            .Select((a, i) => new MenuItem(i + 1, a.Name, true, a)).ToArray();

        return attackActions;
    }

    internal void IncreaseHealthBy(int amount)
    {
        if (CurrentHealth + amount <= MaximumHP)
        {
            CurrentHealth += amount;
        }
        else if (CurrentHealth + amount > MaximumHP)
        {
            CurrentHealth = MaximumHP;
        }
    }

    internal void DecreaseHealth(int damage)
    {
        CurrentHealth -= damage;
    }
}
