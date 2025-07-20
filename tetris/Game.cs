using System.Text;

namespace tetris;

public class Game
{
    public bool IsGameOver { get; private set; }

    private CurrentBlockState _currentBlock;
    private readonly GameGrid _grid;
    private readonly BlockQueue _blockQueue;

    public Game()
    {
        _grid = new GameGrid(22, 10);
        _blockQueue = new BlockQueue();
        _currentBlock = _blockQueue.GetNextBlock();
    }

    private void HandleGameOver()
    {
        IsGameOver = true;
    }

    private void PlaceBlock()
    {
        foreach (var p in _currentBlock.TilePositions())
        {
            _grid.SetCell(p.Row, p.Col, (int)_currentBlock.Block.Type);
        }

        _grid.ClearFullRows();

        if (IsGameOver) return;

        _currentBlock = _blockQueue.GetNextBlock();

        if (!IsBlockPositionValid(_currentBlock))
        {
            HandleGameOver();
        }
    }

    private bool IsBlockPositionValid(CurrentBlockState block)
    {
        foreach (var p in block.TilePositions())
        {
            if (!_grid.IsCellEmpty(p.Row, p.Col))
            {
                return false;
            }
        }
        return true;
    }

    public void MoveBlockLeft()
    {
        var potentialBlock = _currentBlock.WithOffset(0, -1);
        if (IsBlockPositionValid(potentialBlock))
        {
            _currentBlock = potentialBlock;
        }
    }

    public void MoveBlockRight()
    {
        var potentialBlock = _currentBlock.WithOffset(0, 1);
        if (IsBlockPositionValid(potentialBlock))
        {
            _currentBlock = potentialBlock;
        }
    }

    public void RotateBlockClockwise()
    {
        var potentialBlock = _currentBlock.Rotated(B => B.RotateClockwise());
        if (IsBlockPositionValid(potentialBlock))
        {
            _currentBlock = potentialBlock;
        }
    }

    public void RotateBlockCounterClockwise()
    {
        var potentialBlock = _currentBlock.Rotated(B => B.RotateCounterClockwise());
        if (IsBlockPositionValid(potentialBlock))
        {
            _currentBlock = potentialBlock;
        }
    }

    public void MoveBlockDown()
    {
        var potentialBlock = _currentBlock.WithOffset(1, 0);
        if (IsBlockPositionValid(potentialBlock))
        {
            _currentBlock = potentialBlock;
        }
        else
        {
            PlaceBlock();
        }
    }

    public void DropBlock()
    {
        int dropOffset = 0;
        while (IsBlockPositionValid(_currentBlock.WithOffset(dropOffset + 1, 0)))
        {
            dropOffset++;
        }

        _currentBlock = _currentBlock.WithOffset(dropOffset, 0);
        PlaceBlock();
    }

    public void Draw()
    {
        var buffer = new StringBuilder();

        buffer.Append('╔');
        buffer.Append(new string('═', _grid.Cols * 2));
        buffer.Append('╗');
        buffer.AppendLine();

        var blockPositions = _currentBlock.TilePositions().ToHashSet();
        var landingProjection = GetBlockLandingProjection().ToHashSet();

        for (int r = 2; r < _grid.Rows; r++) // Start from row 2 to hide the buffer zone
        {
            buffer.Append('║');
            for (int c = 0; c < _grid.Cols; c++)
            {
                var pos = new Position(r, c);
                if (blockPositions.Contains(pos))
                {
                    buffer.Append(GetBlockString(_currentBlock.Block.Type));
                }
                else if (landingProjection.Contains(pos))
                {
                    buffer.Append("░░");
                }
                else if (_grid.GetCell(r, c) != 0)
                {
                    buffer.Append(GetBlockString((BlockType)_grid.GetCell(r, c)));
                }
                else
                {
                    buffer.Append("  ");
                }
            }
            buffer.Append('║');
            buffer.AppendLine();
        }

        buffer.Append('╚');
        buffer.Append(new string('═', _grid.Cols * 2));
        buffer.Append('╝');

        Console.SetCursorPosition(0, 0);
        Console.Write(buffer.ToString());
    }

    private string GetBlockString(BlockType type) => type switch
    {
        BlockType.I => "II",
        BlockType.J => "JJ",
        BlockType.L => "LL",
        BlockType.O => "OO",
        BlockType.S => "SS",
        BlockType.T => "TT",
        BlockType.Z => "ZZ",
        _ => "  "
    };

    private IEnumerable<Position> GetBlockLandingProjection()
    {
        int dropOffset = 0;
        while (IsBlockPositionValid(_currentBlock.WithOffset(dropOffset + 1, 0)))
        {
            dropOffset++;
        }
        return _currentBlock.WithOffset(dropOffset, 0).TilePositions();
    }

    public void Run()
    {
        var lastTick = DateTime.Now;
        var tickInterval = TimeSpan.FromMilliseconds(500);

        Console.CursorVisible = false;

        while (!IsGameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                HandleInput(key);
                Draw();
            }

            if ((DateTime.Now - lastTick) > tickInterval)
            {
                MoveBlockDown();
                lastTick = DateTime.Now;
                Draw();
            }

            Thread.Sleep(10);
        }

        Console.SetCursorPosition(0, _grid.Rows + 2);
        Console.WriteLine("Game Over!");
    }

    private void HandleInput(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow: MoveBlockLeft(); break;
            case ConsoleKey.RightArrow: MoveBlockRight(); break;
            case ConsoleKey.DownArrow: MoveBlockDown(); break;
            case ConsoleKey.Spacebar: DropBlock(); break;
            case ConsoleKey.Z: RotateBlockCounterClockwise(); break;
            case ConsoleKey.X: RotateBlockClockwise(); break;
            case ConsoleKey.UpArrow: RotateBlockClockwise(); break;
        }
    }
}
