
using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Actions.AttackModifiers;
using TheFinalBattle.Item;

namespace TheFinalBattle.Characters;

public abstract class Character
{
    public abstract CharacterAction[] Actions { get; init; }
    public abstract string Name { get; set; }
    public abstract string Label { get; set; }
    public abstract CharacterAction StandardAttack { get; }
    public Party? Party { get; internal set; }
    public abstract int InitialHP { get; }
    public abstract int MaximumHP { get; }
    protected internal int CurrentHealth { get; protected set; }
    public Gear? EquippedGear { get; set; }
    public abstract IDefenseModifier? DefenseModifier { get; set; }
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

    internal int GetProposedNewHealth(int damage)
    {
        int newHealth = 0;

        if (damage > CurrentHealth)
        {
            newHealth = 0;
        }
        else
        {
            newHealth = CurrentHealth - damage;
        }

        return newHealth;
    }

    internal void RestoreFullHealth()
    {
        CurrentHealth = InitialHP;
    }


    internal void SetHealth(int newHealth)
    {
        CurrentHealth = newHealth;
    }
}
