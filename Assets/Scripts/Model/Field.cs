using System;

namespace Model
{
    public class Field
    {
        private int _rows;
        private int _cols;
        private char[,] _cells;

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
        private const int SYMBOL_START_INDEX = 0x41;
        private const int SYMBOL_END_INDEX = 0x5A;

        public int Rows
        {
            get => _rows;
            private set => _rows = Math.Max(0, value);
        }

        public int Cols
        {
            get => _cols;
            private set => _cols = Math.Max(0, value);
        }

        public int Length => Rows * Cols;

        public char this[int r, int c]
        {
            get
            {
                if (r < 0 || r >= Rows) throw new ArgumentOutOfRangeException($"Argument '{nameof(r)}' out of range [0, {Rows - 1}]");
                if (c < 0 || c >= Cols) throw new ArgumentOutOfRangeException($"Argument '{nameof(c)}' out of range [0, {Cols - 1}]");
                return _cells[r, c];
            }
            private set
            {
                if (r < 0 || r >= Rows) throw new ArgumentOutOfRangeException($"Argument '{nameof(r)}' out of range [0, {Rows - 1}]");
                if (c < 0 || c >= Cols) throw new ArgumentOutOfRangeException($"Argument '{nameof(c)}' out of range [0, {Cols - 1}]");
                _cells[r, c] = value;
            }
        }

        public Field() => new Field(0, 0);

        public Field(int rows, int cols)
        {
            Rows = 0;
            Cols = 0;
            _cells = new char[rows, cols];
            Resize(rows, cols);
        }

        private char RandomSymbol => (char)_random.Next(SYMBOL_START_INDEX, SYMBOL_END_INDEX + 1);

        public void Resize(int rows, int cols)
        {
            if (rows == Rows && cols == Cols) return;

            char[,] temp = new char[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (r < Rows && c < Cols) temp[r, c] = this[r, c];
                    else temp[r, c] = RandomSymbol;
                }
            }
            Rows = rows;
            Cols = cols;
            _cells = temp;
        }

        public int[] Mix()
        {
            int[] coords = new int[Length * 2];
            for(int r = Rows - 1; r >= 0; r--)
            {
                for (int c = Cols - 1; c >= 0; c--)
                {
                    int x = _random.Next(r + 1);
                    int y = _random.Next(c + 1);
                    coords[2 * (r * Cols + c)] = x;
                    coords[2 * (r * Cols + c) + 1] = y;
                    (this[x, y], this[r, c]) = (this[r, c], this[x, y]);
                }
            }
            return coords;
        }
    }
}
