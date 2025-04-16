using TheFinalBattle.Characters;
using TheFinalBattle.Item;

namespace TheFinalBattle.Actions;

public class GearEquipAction : CharacterAction
{
    private readonly Gear _gear;

    public GearEquipAction(Gear gear)
    {
        _gear = gear;
    }

    public Gear Gear { get => _gear; }

    public override string Name
    {
        get
        {
            return $"Equip Gear ({_gear.Name})";
        }
        
    }

    public void EquipGear(Character character)
    {
        _gear.Equipped = true;

        if (character.EquippedGear is not null)
        {
            character.EquippedGear.Equipped = false;
        }

        character.EquippedGear = _gear;
        _gear.EquippedCharacter = character;        
    }
}
