namespace tetris;

public record CurrentBlockState(Block Block, Position Offset)
{
    public IEnumerable<Position> TilePositions() =>
        Block.TilePositions.Select(p => new Position(p.Row + Offset.Row, p.Col + Offset.Col));

    public CurrentBlockState WithOffset(int dRow, int dCol) =>
        this with { Offset = new Position(Offset.Row + dRow, Offset.Col + dCol) };

    public CurrentBlockState Rotated(Action<Block> rotateAction)
    {
        var newBlock = Block.Clone();
        rotateAction(newBlock);
        return this with { Block = newBlock };
    }
}
