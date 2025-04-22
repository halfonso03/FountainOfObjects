using TheFinalBattle.Characters;
using TheFinalBattle.Item;

namespace TheFinalBattle.Actions;

public class GearEquipAction : CharacterAction
{
    public static Dictionary<string, int> CompiledGear { get; set; } = new Dictionary<string, int>();

    public string _internalId = Guid.NewGuid().ToString().Substring(1, 4);

    private readonly Gear _gear;
    public GearEquipAction? AdditionalGearEquipAction { get; set; }
    public GearEquipAction(Gear gear)
    {
        _gear = gear;
    }

    public Gear Gear { get => _gear; }

    public override string Name => $"Equip Gear ({_gear.Name})";


    public GeareEquipActionResult EquipGear(Character character)
    {

        var gearCount = character.EquippedGearItems.Count;
        _gear.Equipped = true;
        
        if (gearCount == 1)
        {
            var currentEquippedGear = character.EquippedGearItems[0];

            if (currentEquippedGear.Pairable && _gear.Pairable)
            {                
                currentEquippedGear.PairedGear = _gear;
                _gear.PairedGear = currentEquippedGear;
            }
            else
            {
                Gear.UnequipFromCharacter(currentEquippedGear);
            }
        }
        else if (gearCount > 1)
        {            
            character.EquippedGearItems.ForEach(g => { g.EquippedCharacter = null; g.Equipped = false; g.PairedGear = null; });
            character.EquippedGearItems.RemoveAll(x => 1 == 1);
                     
        }

        _gear.EquippedCharacter = character;
        character.EquipGear(_gear);


        // applies to the computer player only
        if (AdditionalGearEquipAction is not null)
        {
            GearActionCompile(AdditionalGearEquipAction.EquipGear(character));
        }

        return new GeareEquipActionResult() { EquippedGear = Gear };        
    }

    public static void GearActionCompile(GeareEquipActionResult actionResult)
    {
        if (!CompiledGear.TryGetValue(actionResult.EquippedGear.Name, out int value))
        {
            value = 0;
            CompiledGear[actionResult.EquippedGear.Name] = value;
        }
        CompiledGear[actionResult.EquippedGear.Name] = ++value;
    }

    public override string ToString() => _internalId;
}


public class GeareEquipActionResult
{
    public Gear EquippedGear { get; set; }
}