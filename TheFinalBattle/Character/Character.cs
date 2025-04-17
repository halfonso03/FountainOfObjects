
using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Item;

namespace TheFinalBattle.Characters;

public abstract class Character
{
    public abstract CharacterAction[] Actions { get; init; }
    public abstract string Name { get; set; }
    public abstract string Label { get; set; }
    public abstract CharacterAction StandardAttack { get; }
    public Party Party { get; internal set; }
    public abstract int InitialHP { get; }
    public abstract int MaximumHP { get; }
    protected internal int CurrentHealth { get; set; }
    public Gear? EquippedGear { get; set; }
    public abstract IAttackModifier? AttackModifier { get; set; }
    public MenuItem[] GetStandardAttackMenuOptions()
    {
        var attackActions = Actions
            .OfType<StandardAttackAction>()
            .Select((action, i) => new MenuItem(i + 1, action.Name, true, action)).ToArray();

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

        if (CurrentHealth > InitialHP)
        {
            throw new Exception("Charactr hp cannot exceed initial HP!");
        }
    }
}
