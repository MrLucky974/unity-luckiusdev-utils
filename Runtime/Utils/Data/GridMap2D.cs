using System;
using UnityEngine;

namespace LuckiusDev.Utils.Data
{
    public class GridMap2D<TGridObject> : GridMapBase<TGridObject>
    {
        private readonly Vector2 m_originPosition;
        public Vector2 WorldOrigin => m_originPosition;

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

        public Vector2 WorldToAlignedWorld(Vector2 worldPosition, bool toCellCenter = true)
        {
            Vector2Int gridPos = WorldToGrid(worldPosition);
            return GridToWorld(gridPos, toCellCenter);
        }

        public Vector2 GridToWorld(int x, int y, bool toCellCenter = true)
        {
            var position = new Vector2(x, y) * cellSize + m_originPosition;

            if (toCellCenter)
            {
                position += Vector2.one * cellSize * 0.5f;
            }
            return position;
        }

        public Vector2 GridToWorld(Vector2Int gridPos, bool toCellCenter = true)
        {
            return GridToWorld(gridPos.x, gridPos.y, toCellCenter);
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