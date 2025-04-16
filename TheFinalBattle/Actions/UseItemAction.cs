using TheFinalBattle.Characters;

namespace TheFinalBattle.Actions;

public class UseItemAction : CharacterAction
{
    private readonly IItem _item;

    public UseItemAction(IItem item)
    {
        _item = item;
    }

    public IItem Item { get => _item; }

    public override string Name
    {
        get
        {
            return $"Use Item ({_item.Name})";
        }
        set {}
    }

    public void UseItem(Character character)
    {
        _item.UseItem(character);
    }
}
