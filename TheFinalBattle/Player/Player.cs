using TheFinalBattle.Characters;

namespace TheFinalBattle.Player;

public interface IPlayer
{
    string Name { get; set; }
    IParty[] Parties { get; init; }
    PlayerType PlayerType { get; set; }
    Character? GetHealthRestoreCandidate();
}

public class Player : IPlayer
{
    public IParty[] Parties { get; init; }
    public PlayerType PlayerType { get; set; } = PlayerType.Computer;
    public string Name { get; set; }

    public Player(IParty[] parties)
    {
        Parties = parties;
    }

    public Character? GetHealthRestoreCandidate()
    {
        Character? character = Parties.SelectMany(x => x.GetCharacters()).MinBy(x => x.CurrentHealth);

        return (character is not null && character.CurrentHealth <= character.MaximumHP * .2) 
            ? character 
            : null;
    }

    internal void ClearParty()
    {
        throw new NotImplementedException();
    }
}
