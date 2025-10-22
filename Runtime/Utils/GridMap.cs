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
        public event EventHandler<OnGridValueChangedEventArgs> onGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        private readonly int m_width, m_height;
        private readonly float m_cellSize;
        private readonly Vector3 m_originPosition;
        private readonly TGridObject[,] m_grid;

        public int Width => m_width;
        public int Height => m_height;
        public float CellSize => m_cellSize;

        /// <summary>
        /// Initializes the grid with specified dimensions, cell size, origin position, and object creation function.
        /// </summary>
        public GridMap(int width, int height, float cellSize, Vector3 originPosition,
            Func<GridMap<TGridObject>, int, int, TGridObject> createGridObject)
        {
            m_width = width;
            m_height = height;
            m_cellSize = cellSize;
            m_originPosition = originPosition;

            m_grid = new TGridObject[width, height];

            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    m_grid[x, y] = createGridObject(this, x, y);
                }
            }
        }

        #region Position Conversion

        /// <summary>
        /// Converts grid coordinates to a world space position.
        /// </summary>
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * m_cellSize + m_originPosition;
        }

        /// <summary>
        /// Converts a world position into grid coordinates (out as integers).
        /// </summary>
        public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - m_originPosition).x / m_cellSize);
            y = Mathf.FloorToInt((worldPosition - m_originPosition).z / m_cellSize);
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
                return m_grid[x, y];
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
                m_grid[x, y] = value;
                TriggerGridObjectChanged(x, y);
            }
        }

        /// <summary>
        /// Triggers the change event for the specified cell.
        /// </summary>
        public void TriggerGridObjectChanged(int x, int y)
        {
            onGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Checks if the specified grid coordinates are within the grid bounds.
        /// </summary>
        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < m_width && y < m_height;
        }

        /// <summary>
        /// Executes an action for each cell in the grid.
        /// </summary>
        public void ForEach(Action<int, int, TGridObject> action)
        {
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    action.Invoke(x, y, m_grid[x, y]);
                }
            }
        }

        /// <summary>
        /// Returns a reference to the underlying 2D array of grid values.
        /// </summary>
        public TGridObject[,] GetRawGrid()
        {
            return m_grid;
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
