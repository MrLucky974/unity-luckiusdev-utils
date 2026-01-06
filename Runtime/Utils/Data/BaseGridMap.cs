using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils.Data
{
    public abstract class GridMapBase<TGridObject>
    {
        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        protected readonly int width;
        protected readonly int height;
        protected readonly float cellSize;
        protected readonly TGridObject[,] grid;

        public int Width => width;
        public int Height => height;
        public float CellSize => cellSize;

        protected GridMapBase(
            int width,
            int height,
            float cellSize,
            Func<GridMapBase<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;

            grid = new TGridObject[width, height];

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    grid[x, y] = createGridObject(this, x, y);
                }
            }
        }

        #region Grid Access

        public TGridObject GetValue(int x, int y)
        {
            return IsInBounds(x, y) ? grid[x, y] : default;
        }

        public void SetValue(int x, int y, TGridObject value)
        {
            if (!IsInBounds(x, y)) return;

            grid[x, y] = value;
            TriggerGridObjectChanged(x, y);
        }

        protected void TriggerGridObjectChanged(int x, int y)
        {
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }

        #endregion

        #region Utilities

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        public void ForEach(Action<int, int, TGridObject> action)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    action(x, y, grid[x, y]);
                }
            }
        }

        public TGridObject[,] GetRawGrid() => grid;

        public IEnumerable<Vector2Int> GetNeighbors(int x, int y, bool includeDiagonals = false)
        {
            static IEnumerable<Vector2Int> Cardinal()
            {
                yield return new Vector2Int(1, 0);
                yield return new Vector2Int(-1, 0);
                yield return new Vector2Int(0, 1);
                yield return new Vector2Int(0, -1);
            }

            static IEnumerable<Vector2Int> Diagonal()
            {
                yield return new Vector2Int(1, 1);
                yield return new Vector2Int(1, -1);
                yield return new Vector2Int(-1, 1);
                yield return new Vector2Int(-1, -1);
            }

            foreach (var dir in Cardinal())
            {
                int nx = x + dir.x;
                int ny = y + dir.y;
                if (IsInBounds(nx, ny)) yield return new Vector2Int(nx, ny);
            }

            if (!includeDiagonals) yield break;

            foreach (var dir in Diagonal())
            {
                int nx = x + dir.x;
                int ny = y + dir.y;
                if (IsInBounds(nx, ny)) yield return new Vector2Int(nx, ny);
            }
        }

        #endregion
    }
}