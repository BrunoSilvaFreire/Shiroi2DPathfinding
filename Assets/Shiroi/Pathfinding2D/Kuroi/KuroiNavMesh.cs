using System;
using Shiroi.Pathfinding2D.Runtime;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Kuroi {
    public class KuroiNavMesh : NavMesh2D<KuroiNavMesh.GeometryNode> {
        public Vector2 boxCastSize;

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

            public bool IsEmpty() {
                return flags == 0;
            }
        }

        private bool Test(int x, int y) {
            var position = grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            var found = Physics2D.OverlapBox(position, boxCastSize, 0, worldMask);
            return found;
        }

        public override GeometryNode GenerateNode(int x, int y) {
            GeometryNode.NodeFlags flags = 0;
            if (Test(x, y)) {
                flags |= GeometryNode.NodeFlags.Solid;
            } else {
                var supported = false;
                if (Test(x, y - 1)) {
                    flags |= GeometryNode.NodeFlags.Supported;
                    supported = true;
                }

                if (Test(x - 1, y)) {
                    flags |= GeometryNode.NodeFlags.LeftWall;
                } else {
                    if (supported && !Test(x - 1, y - 1)) {
                        flags |= GeometryNode.NodeFlags.LeftEdge;
                    }
                }

                if (Test(x + 1, y)) {
                    flags |= GeometryNode.NodeFlags.RightWall;
                } else {
                    if (supported && !Test(x + 1, y - 1)) {
                        flags |= GeometryNode.NodeFlags.RightEdge;
                    }
                }
            }

            return new GeometryNode {
                flags = flags
            };
        }
    }
}