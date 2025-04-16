
using TheFinalBattle.Actions;
using TheFinalBattle.Item;

namespace TheFinalBattle.Characters;

public abstract class Character
{
    public abstract CharacterAction[] Actions { get; init; }
    public abstract string Name { get; set; }
    public abstract CharacterAction StandardAttack { get; }
    public Party Party { get; internal set; }
    public abstract int InitialHP { get; }
    public abstract int MaximumHP { get; }
    protected internal int CurrentHealth { get; set; }
    public MenuItem[] GetAttackMenu()
    {
        var attackActions = Actions
            .OfType<StandardAttackAction>()
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

    internal void DecreaseHealthBy(int damage)
    {
        if (damage > CurrentHealth)
        {
            CurrentHealth = 0;
        }
        else
        {
            CurrentHealth -= damage;
        }
    }

    public Gear? EquippedGear { get; set; }
}
