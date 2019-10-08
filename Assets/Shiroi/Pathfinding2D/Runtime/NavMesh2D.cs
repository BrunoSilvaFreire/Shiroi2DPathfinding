using System;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Runtime {
    public abstract class NavMesh2D<G> : MonoBehaviour {
        [SerializeField, HideInInspector]
        private G[] nodes;

        public LayerMask worldMask;
        public Grid grid;

        [SerializeField]
        private Vector2Int min, max;

        public G[] Nodes => nodes;

        public void SetMinMax(Vector2Int a, Vector2Int b) {
            min = new Vector2Int(
                Math.Min(a.x, b.x),
                Math.Min(a.y, b.y)
            );
            max = new Vector2Int(
                Math.Max(a.x, b.x),
                Math.Max(a.y, b.y)
            );
        }

        public Vector2Int Min {
            get { return min; }
            set { SetMinMax(value, max); }
        }

        public Vector2Int Max {
            get { return max; }
            set { SetMinMax(value, min); }
        }

        public Vector3 MinWorld => grid.GetCellCenterWorld((Vector3Int) min);
        public Vector3 MaxWorld => grid.GetCellCenterWorld((Vector3Int) max);
        public uint Width => (uint) (max.x - min.x + 1);
        public uint Height => (uint) (max.y - min.y + 1);
        public uint Area => Width * Height;

        public uint IndexOf(int x, int y) {
            if (IsOutOfBounds(x, y)) {
                throw new PositionOutOfBoundsException(x, y);
            }

            return IndexOfUnsafe(x, y);
        }

        public bool IsOutOfBounds(int x, int y) {
            return x > max.x || x < min.x || y > max.y || y < min.y;
        }

        public uint IndexOfUnsafe(int x, int y) {
            var transformedX = x - min.x;
            var transformedY = y - min.y;
            return (uint) (transformedY * Width + transformedX);
        }

        public void ImportNodes(G[] newNodes) {
            nodes = newNodes;
        }

        public Vector3 WorldCenter(int x, int y) {
            return grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
        }

        public G NodeUnsafe(int cellX, int cellY) {
            return nodes[IndexOfUnsafe(cellX, cellY)];
        }

        public Vector2Int PositionOf(uint index) {
            var x = index % Width;
            var y = index / Width;
            return new Vector2Int((int) (min.x + x), (int) (min.y + y));
        }

        public bool IsOutOfBounds(Vector2 currentPosition) {
            var cell = grid.WorldToCell(currentPosition);
            return IsOutOfBounds(cell.x, cell.y);
        }

        public abstract G GenerateNode(int x, int y);
    }

    public class PositionOutOfBoundsException : Exception {
        private readonly int x;
        private readonly int y;

        public PositionOutOfBoundsException(int x, int y) : base($"Position {x}, {y} is out of bounds of navmesh") {
            this.x = x;
            this.y = y;
        }
    }
}