namespace TheFinalBattle.Player;

public class Player
{
    public IParty[] Parties { get; init; }
    public PlayerType PlayerType { get; set; } = PlayerType.Computer;
    public Player(IParty[] parties)
    {
        Parties = parties;
    }
}
