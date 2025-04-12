using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// A generic 2D grid data structure for mapping grid coordinates to values of type TGridObject.
    /// Includes position conversion methods, change notification events, and helpful grid utilities.
    /// </summary>
    public class GridMap<TGridObject>
    {
        /// <summary>
        /// Event triggered when a cell value changes in the grid.
        /// </summary>
        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int X;
            public int Y;
        }

        private readonly int _width, _height;
        private readonly float _cellSize;
        private readonly Vector3 _originPosition;
        private readonly TGridObject[,] _grid;

        public int Width => _width;
        public int Height => _height;
        public float CellSize => _cellSize;

        /// <summary>
        /// Initializes the grid with specified dimensions, cell size, origin position, and object creation function.
        /// </summary>
        public GridMap(int width, int height, float cellSize, Vector3 originPosition,
            Func<GridMap<TGridObject>, int, int, TGridObject> createGridObject)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _originPosition = originPosition;

            _grid = new TGridObject[width, height];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _grid[x, y] = createGridObject(this, x, y);
                }
            }
        }

        #region Position Conversion

        /// <summary>
        /// Converts grid coordinates to a world space position.
        /// </summary>
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * _cellSize + _originPosition;
        }

        /// <summary>
        /// Converts a world position into grid coordinates (out as integers).
        /// </summary>
        public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
        }

        /// <summary>
        /// Converts a world position to grid coordinates as a Vector2Int.
        /// </summary>
        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out int x, out int y);
            return new Vector2Int(x, y);
        }

        #endregion

        #region Grid Accessors

        /// <summary>
        /// Returns the value at the specified grid coordinates.
        /// </summary>
        public TGridObject GetValue(int x, int y)
        {
            if (IsInBounds(x, y))
            {
                return _grid[x, y];
            }
            return default;
        }

        /// <summary>
        /// Sets the value at the specified grid coordinates and triggers change notification.
        /// </summary>
        public void SetValue(int x, int y, TGridObject value)
        {
            if (IsInBounds(x, y))
            {
                _grid[x, y] = value;
                TriggerGridObjectChanged(x, y);
            }
        }

        /// <summary>
        /// Triggers the change event for the specified cell.
        /// </summary>
        public void TriggerGridObjectChanged(int x, int y)
        {
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { X = x, Y = y });
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Checks if the specified grid coordinates are within the grid bounds.
        /// </summary>
        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        }

        /// <summary>
        /// Executes an action for each cell in the grid.
        /// </summary>
        public void ForEach(Action<int, int, TGridObject> action)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    action.Invoke(x, y, _grid[x, y]);
                }
            }
        }

        /// <summary>
        /// Returns a reference to the underlying 2D array of grid values.
        /// </summary>
        public TGridObject[,] GetRawGrid()
        {
            return _grid;
        }

        /// <summary>
        /// Gets neighboring cell coordinates from a given position (optionally including diagonals).
        /// </summary>
        public IEnumerable<Vector2Int> GetNeighbors(int x, int y, bool includeDiagonals = false)
        {
            var directions = new List<Vector2Int>
            {
                new(1, 0), new(-1, 0), new(0, 1), new(0, -1)
            };

            if (includeDiagonals)
            {
                directions.AddRange(new[]
                {
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                    new Vector2Int(-1, 1),
                    new Vector2Int(-1, -1)
                });
            }

            foreach (var dir in directions)
            {
                int nx = x + dir.x;
                int ny = y + dir.y;

                if (IsInBounds(nx, ny))
                {
                    yield return new Vector2Int(nx, ny);
                }
            }
        }

        #endregion
    }
}
