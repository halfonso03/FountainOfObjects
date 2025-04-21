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

    public override string Name
    {
        get
        {
            return $"Equip Gear ({_gear.Name})";
        }
        
    }

    public GeareEquipActionResult EquipGear(Character character)
    {

        var gearCount = character.EquippedGearItems.Count;

        if (gearCount == 0)
        {
            _gear.Equipped = true;

            character.EquipGear(_gear);

            _gear.EquippedCharacter = character;
        }        
        else if (gearCount == 1)
        {
            var currentGear = character.EquippedGearItems[0];

            if (currentGear.Pairable && _gear.Pairable)
            {
                _gear.Equipped = true;

                character.EquipGear(_gear);

                _gear.EquippedCharacter = character;

                currentGear.PairedGear = _gear;
                _gear.PairedGear = currentGear;
            }
            else
            {
                Gear.UnequipCharacter(currentGear);

                _gear.Equipped = true;

                character.EquipGear(_gear);

                _gear.EquippedCharacter = character;
            }
        }
        else
        {
            var equippedGear = character.EquippedGearItems;

            equippedGear.ForEach(g => { g.EquippedCharacter = null; g.Equipped = false; g.PairedGear = null; });
            character.EquippedGearItems.RemoveAll(x => 1 == 1);
            
            _gear.Equipped = true;

            character.EquipGear(_gear);

            _gear.EquippedCharacter = character;
        }

        if (AdditionalGearEquipAction is not null)
        {
            GearActionCompile(AdditionalGearEquipAction.EquipGear(character));
        }




        var actionResult = new GeareEquipActionResult() { EquippedGear = Gear };

        return actionResult;
        
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

    //public void EquipGear_OLD(Character character)
    //{
    //    _gear.Equipped = true;

    //    if (character.EquippedGear is not null)
    //    {
    //        character.EquippedGear.Equipped = false;
    //    }

    //    character.EquippedGear = _gear;


    //    character.EquipGear(_gear);

    //    _gear.EquippedCharacter = character;

    //    if (this.AdditionalGearEquipAction is not null)
    //    {
    //        AdditionalGearEquipAction.EquipGear(character);
    //    }
    //}

    public override string ToString() => _internalId;
}


public class GeareEquipActionResult
{
    public Gear EquippedGear { get; set; }
}