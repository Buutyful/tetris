namespace tetris;

public enum BlockType
{
    I = 1, J, L, O, S, T, Z
}

public record struct Position(int Row, int Col);

public class Block
{
    public int RotationState { get; private set; } = 0;

    // Defines the tile positions for each rotation state of a block.
    // Based on the Super Rotation System (SRS) used in modern Tetris games.
    private readonly Position[][] _positions = new Position[4][];

    public BlockType Type { get; }

    public Block(BlockType type)
    {
        Type = type;
        _positions = type switch
        {
            BlockType.I =>
            [
                [new(1, 0), new(1, 1), new(1, 2), new(1, 3)],
                [new(0, 2), new(1, 2), new(2, 2), new(3, 2)],
                [new(2, 0), new(2, 1), new(2, 2), new(2, 3)],
                [new(0, 1), new(1, 1), new(2, 1), new(3, 1)]
            ],
            BlockType.J =>
            [
                [new(0, 0), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(0, 2), new(1, 1), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 2)],
                [new(0, 1), new(1, 1), new(2, 0), new(2, 1)]
            ],
            BlockType.L =>
            [
                [new(0, 2), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(2, 1), new(2, 2)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 0)],
                [new(0, 0), new(0, 1), new(1, 1), new(2, 1)]
            ],
            BlockType.O =>
            [
                [new(0, 0), new(0, 1), new(1, 0), new(1, 1)],
                [new(0, 0), new(0, 1), new(1, 0), new(1, 1)],
                [new(0, 0), new(0, 1), new(1, 0), new(1, 1)],
                [new(0, 0), new(0, 1), new(1, 0), new(1, 1)]
            ],
            BlockType.S =>
            [
                [new(0, 1), new(0, 2), new(1, 0), new(1, 1)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 2)],
                [new(1, 1), new(1, 2), new(2, 0), new(2, 1)],
                [new(0, 0), new(1, 0), new(1, 1), new(2, 1)]
            ],
            BlockType.T =>
            [
                [new(0, 1), new(1, 0), new(1, 1), new(1, 2)],
                [new(0, 1), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(1, 2), new(2, 1)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 1)]
            ],
            BlockType.Z =>
            [
                [new(0, 0), new(0, 1), new(1, 1), new(1, 2)],
                [new(0, 2), new(1, 1), new(1, 2), new(2, 1)],
                [new(1, 0), new(1, 1), new(2, 1), new(2, 2)],
                [new(0, 1), new(1, 0), new(1, 1), new(2, 0)]
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(type), "Invalid block type")
        };
    }

    public IEnumerable<Position> TilePositions => _positions[RotationState];

    public void RotateClockwise()
    {
        RotationState = (RotationState + 1) % _positions.Length;
    }

    public void RotateCounterClockwise()
    {
        RotationState = (RotationState == 0) ? _positions.Length - 1 : RotationState - 1;
    }

    public Block Clone() => new(Type) 
    {
        RotationState = RotationState
    };
}