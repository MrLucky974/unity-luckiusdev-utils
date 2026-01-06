using System;
using UnityEngine;

namespace LuckiusDev.Utils.Data
{
    public class GridMap2D<TGridObject> : GridMapBase<TGridObject>
    {
        private readonly Vector2 m_originPosition;

        public GridMap2D(
            int width,
            int height,
            float cellSize,
            Vector2 originPosition,
            Func<GridMap2D<TGridObject>, int, int, TGridObject> createGridObject)
            : base(width, height, cellSize,
                (grid, x, y) => createGridObject((GridMap2D<TGridObject>)grid, x, y))
        {
            m_originPosition = originPosition;
        }

        public Vector2 GetWorldPosition(int x, int y)
        {
            return new Vector2(x, y) * cellSize + m_originPosition;
        }

        public Vector2Int WorldToGrid(Vector2 worldPosition)
        {
            GetGridPosition(worldPosition, out var x, out var y);
            return new Vector2Int(x, y);
        }

        private void GetGridPosition(Vector2 worldPosition, out int x, out int y)
        {
            Vector2 local = worldPosition - m_originPosition;
            x = Mathf.FloorToInt(local.x / cellSize);
            y = Mathf.FloorToInt(local.y / cellSize);
        }
    }
}