using System;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Runtime {
    public class NavMesh2D : MonoBehaviour {
        [Serializable]
        public struct GeometryNode {
            public NodeFlags flags;

            [Flags]
            public enum NodeFlags : byte {
                /// <summary>
                /// Is this node obstructed?
                /// </summary>
                Solid = 1 << 0,

                /// <summary>
                /// Is the node below this one obstructed?
                /// </summary>
                Supported = 1 << 1,

                /// <summary>
                /// Is the node left to this one obstructed?
                /// </summary>
                LeftWall = 1 << 2,

                /// <summary>
                /// Is the node right to this one obstructed?
                /// </summary>
                RightWall = 1 << 3,

                /// <summary>
                /// Is the node left and bottom to this one not obstructed?
                /// </summary>
                LeftEdge = 1 << 4,

                /// <summary>
                /// Is the node right and bottom to this one not obstructed?
                /// </summary>
                RightEdge = 1 << 5,
                UnusedA = 1 << 6,
                UnusedB = 1 << 7
            }

            public bool IsSupported() {
                return Is(NodeFlags.Supported);
            }

            public bool IsSolid() {
                return Is(NodeFlags.Solid);
            }

            public bool IsLeftWall() {
                return Is(NodeFlags.LeftWall);
            }

            public bool IsRightWall() {
                return Is(NodeFlags.RightWall);
            }

            private bool Is(NodeFlags mask) {
                return (flags & mask) == mask;
            }

            public bool IsLeftEdge() {
                return Is(NodeFlags.LeftEdge);
            }

            public bool IsRightEdge() {
                return Is(NodeFlags.RightEdge);
            }
        }

        [SerializeField, HideInInspector]
        private GeometryNode[] nodes;

        public LayerMask worldMask;
        public Grid grid;

        [SerializeField]
        private Vector2Int min, max;

        public GeometryNode[] Nodes => nodes;

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
#if UNITY_EDITOR

        public void ImportNodes(GeometryNode[] newNodes) {
            nodes = newNodes;
        }
#endif
        public Vector3 WorldCenter(int x, int y) {
            return grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
        }

        public GeometryNode NodeUnsafe(int cellX, int cellY) {
            return nodes[IndexOfUnsafe(cellX, cellY)];
        }

        public Vector2Int PositionOf(uint index) {
            var x = index % Width;
            var y = index / Width;
            return new Vector2Int((int) (min.x + x), (int) (min.y + y));
        }
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