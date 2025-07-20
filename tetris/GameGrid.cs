namespace tetris;

public class GameGrid(int rows, int cols)
{
    private readonly int[,] _grid = new int[rows, cols];
    public int Rows { get; } = rows;
    public int Cols { get; } = cols;
    public int this[int r, int c]
    {
        get => GetCell(r, c);
        set => SetCell(r, c, value);
    }
    private bool IsInside(int r, int c)
    {
        return r >= 0 && r < Rows && c >= 0 && c < Cols;
    }

    public bool IsCellEmpty(int r, int c)
    {
        return IsInside(r, c) && _grid[r, c] == 0;
    }

    public int GetCell(int r, int c)
    {
        if (!IsInside(r, c)) return -1;
        return _grid[r, c];
    }

    public void SetCell(int r, int c, int value)
    {
        if (IsInside(r, c))
        {
            _grid[r, c] = value;
        }
    }

    private bool IsRowFull(int r)
    {
        for (int c = 0; c < Cols; c++)
        {
            if (_grid[r, c] == 0) return false;
        }
        return true;
    }

    private void ClearRow(int r)
    {
        for (int c = 0; c < Cols; c++)
        {
            _grid[r, c] = 0;
        }
    }

    private void MoveRowDown(int r, int numRows)
    {
        for (int c = 0; c < Cols; c++)
        {
            _grid[r + numRows, c] = _grid[r, c];
            _grid[r, c] = 0;
        }
    }

    public int ClearFullRows()
    {
        int cleared = 0;
        for (int r = Rows - 1; r >= 0; r--)
        {
            if (IsRowFull(r))
            {
                ClearRow(r);
                cleared++;
            }
            else if (cleared > 0)
            {
                MoveRowDown(r, cleared);
            }
        }
        return cleared;
    }
}