using System;
using UnityEngine;

namespace LuckiusDev.Utils.Data
{
    public class GridMap<TGridObject> : GridMapBase<TGridObject>
    {
        private readonly Vector3 m_originPosition;
        public Vector3 WorldOrigin => m_originPosition;

        public GridMap(
            int width,
            int height,
            float cellSize,
            Vector3 originPosition,
            Func<GridMap<TGridObject>, int, int, TGridObject> createGridObject)
            : base(width, height, cellSize,
                (grid, x, y) => createGridObject((GridMap<TGridObject>)grid, x, y))
        {
            m_originPosition = originPosition;
        }

        public Vector3 WorldToAlignedWorld(Vector3 worldPosition, bool toCellCenter = true)
        {
            Vector2Int gridPos = WorldToGrid(worldPosition);
            return GridToWorld(gridPos, toCellCenter);
        }

        public Vector3 GridToWorld(int x, int y, bool toCellCenter = true)
        {
            var position = new Vector3(x, 0f, y) * cellSize + m_originPosition;

            if (toCellCenter)
            {
                position += new Vector3(0.5f, 0f, 0.5f) * cellSize;
            }

            return position;
        }

        public Vector3 GridToWorld(Vector2Int gridPos, bool toCellCenter = true)
        {
            return GridToWorld(gridPos.x, gridPos.y, toCellCenter);
        }

        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            GetGridPosition(worldPosition, out int x, out int y);
            return new Vector2Int(x, y);
        }

        private void GetGridPosition(Vector3 worldPosition, out int x, out int y)
        {
            Vector3 local = worldPosition - m_originPosition;
            x = Mathf.FloorToInt(local.x / cellSize);
            y = Mathf.FloorToInt(local.z / cellSize);
        }
    }
}