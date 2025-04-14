var bc1 = new BlockCoordinate(0, 0);
var bo2 = new BlockOffset(1, 1);


BlockCoordinate bc3 = bc1 + bo2;

//Console.WriteLine(bc3);

//Console.WriteLine(bc1);
//Console.WriteLine(bc1 + Direction.North);


//Console.WriteLine(bc1[1]);

var newBo = Direction.North;

BlockOffset bo3 = Direction.North;


Console.WriteLine(bo3);

Console.ReadKey();


public record BlockCoordinate(int Row, int Column)
{
    public static BlockCoordinate operator +(BlockCoordinate blockCoordinate, BlockOffset blockOffset)
    {
        return
            new BlockCoordinate(
                blockCoordinate.Row + blockOffset.RowOffset,
                blockCoordinate.Column + blockOffset.ColumnOffset);
    }

    public static BlockCoordinate operator +(BlockCoordinate blockCoordinate, Direction direction)
    {
        return direction switch
        {
            Direction.North => blockCoordinate  + new BlockOffset(1,0),
            Direction.South => blockCoordinate  + new BlockOffset(-1,0),
            Direction.East => blockCoordinate  + new BlockOffset(0,1),
            Direction.West => blockCoordinate  + new BlockOffset(0,-1),
            _ => blockCoordinate
        };
            
    }

    public int this[int index]
    {
        get
        {
            if (index == 0) return Row;
            if (index == 1) return Column;
            return -1;
        }
    }
}




public record BlockOffset(int RowOffset, int ColumnOffset)
{
    public static implicit operator BlockOffset(Direction direction)
    {
        return direction switch
        {
            Direction.North => new BlockOffset(-1, 0),
            Direction.South => new BlockOffset(1, 0),
            Direction.East => new BlockOffset(0, 1),
            Direction.West => new BlockOffset(0, -1),
        };
    }
}
public enum Direction { North, East, South, West }








