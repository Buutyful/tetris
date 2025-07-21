namespace tetris;

public class BlockQueue
{
    private readonly Queue<Block> _blocks = new();
    private readonly Random _random = new();

    public BlockQueue()
    {
        FillQueue();
    }

    private void FillQueue()
    {
        var blockTypes = Enum.GetValues<BlockType>().ToList();

        // 7-bag randomizer shuffle
        for (int i = blockTypes.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (blockTypes[i], blockTypes[j]) = (blockTypes[j], blockTypes[i]);
        }

        foreach (var type in blockTypes)
        {
            _blocks.Enqueue(new Block(type));
        }
    }

    public CurrentBlockState GetNextBlock()
    {
        if (_blocks.Count == 0)
        {
            FillQueue();
        }
        var block = _blocks.Dequeue();
        var spawnOffset = new Position(0, block.Type == BlockType.I ? 3 : 4);
        return new CurrentBlockState(block, spawnOffset);
    }
}