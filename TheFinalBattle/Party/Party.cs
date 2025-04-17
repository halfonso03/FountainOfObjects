using TheFinalBattle.Actions;
using TheFinalBattle.Actions.AttackActions;
using TheFinalBattle.Characters;
using TheFinalBattle.Item;

public interface IParty
{
    int CharacterCount { get; }
    List<IItem> Items { get; set; }
    List<Gear> AttackGear { get; set; }
    PartyType PartyType { get; init; }
    Character GetCharacterAt(int index);
    Character GetNextCharacter();
    void RemoveCharacter(Character enemyCharacter);
    IEnumerable<Character> GetCharacters(Func<Character, bool> predicate);
    IEnumerable<Character?> GetCharacters();
    IEnumerable<MenuItem> GetAvailableItemActionsMenu(int startIndex = 1);
    IEnumerable<MenuItem> GetEquippableGearMenu(int startIndex = 1);
    MenuItem? GetEquippedGearAttackOption(Character character, int startIndex = 1);
    void AddItems(List<IItem> items);
    IEnumerable<Gear> GetEquippableGear();
    IEnumerable<Gear> GetEquippedGear();
    Character GetNextAttackingCharacter();
    Character GetCurrentAttackingCharacter();
}


public class Party : IParty
{
    public PartyType PartyType { get; init; } = PartyType.Hero;
    private List<Character> _characters { get; set; } = [];
    public int CharacterCount => _characters.Count;

    public int CurrentAttackingCharacterIndex = 0;
    public List<IItem> Items { get; set; } = [];
    public List<Gear> AttackGear { get; set; } = [];

    public Party(int characterCount)
    {
        for (int i = 0; i < characterCount; i++)
        {
            var skeleton = new Skeleton() { Party = this };
            _characters.Add(skeleton);
        }
    }

    public Party(List<Character> characters)
    {
        _characters = characters;
        foreach (var c in _characters)
        {
            c.Party = this;
            if (c.EquippedGear != null)
            {
                AttackGear.Add(c.EquippedGear);
            }
        }
    }

    public Character GetCharacterAt(int index) => _characters[index];
    public IEnumerable<Character> GetCharacters(Func<Character, bool> predicate) => _characters.Where(predicate);
    public void RemoveCharacter(Character enemyCharacter)
    {
        if (enemyCharacter.EquippedGear is not null)
        {            
            Gear.UnequipCharacter(enemyCharacter?.EquippedGear);
        }
        _characters.Remove(enemyCharacter);
        
    }
    public IEnumerable<Character?> GetCharacters() => _characters.AsEnumerable();
    public Character GetNextCharacter()
    {
        CurrentAttackingCharacterIndex++;

        if (CurrentAttackingCharacterIndex >= _characters.Count)
        {
            CurrentAttackingCharacterIndex = 0;
        }

        return _characters[CurrentAttackingCharacterIndex];
    }

    public IEnumerable<MenuItem> GetAvailableItemActionsMenu(int startIndex = 1)
    {
        return Items
            .Where(x => !x.Used)
            .Select((item, i) =>
                new MenuItem(i + startIndex, item.Name, true, new UseItemAction(item)));
    }

    public IEnumerable<Gear> GetEquippableGear()
    {
        return AttackGear
            .Where(x => !x.Equipped)
            .Select(x => x);
    }

    public IEnumerable<Gear> GetEquippedGear()
    {
        return AttackGear
           .Where(x => x.Equipped)
           .Select(x => x);
    }

    public IEnumerable<MenuItem> GetEquippableGearMenu(int startIndex = 1)
    {
        return GetEquippableGear()
                .Select((item, i) =>
                    new MenuItem(i + startIndex, item.Name, true, new GearEquipAction(item)));
    }

    public MenuItem? GetEquippedGearAttackOption(Character character, int startIndex = 1)
    {
        return AttackGear
            .Where(x => x.Equipped && x.EquippedCharacter == character)
            .Select((item, i) =>
                new MenuItem(i + startIndex, item.Name, true, new GearAttackAction(item)))
             .SingleOrDefault();
    }

    public void AddItems(List<IItem> items)
    {
        foreach (var item in items)
        {
            if (!item.Used)
            {
                Items.Add(item);   
            }
        }
    }

    int _characterAttackIndexTracker = 0;

    public Character GetNextAttackingCharacter()
    {
        var character = _getNextAttackingCharacter().First();
        return character;
    }

    private Character? _currentAttackingCharacter;
    private IEnumerable<Character> _getNextAttackingCharacter()
    {        
        var character = _characters[_characterAttackIndexTracker % _characters.Count];
        _currentAttackingCharacter = character;
        _characterAttackIndexTracker++;
        yield return character; 
    }

    public Character GetCurrentAttackingCharacter()
    {
        return _currentAttackingCharacter;
    }
}
