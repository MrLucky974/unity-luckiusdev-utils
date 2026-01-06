using System;
using UnityEngine;

namespace LuckiusDev.Utils.Data
{
    public class GridMap<TGridObject> : GridMapBase<TGridObject>
    {
        private readonly Vector3 m_originPosition;

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

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0f, y) * cellSize + m_originPosition;
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