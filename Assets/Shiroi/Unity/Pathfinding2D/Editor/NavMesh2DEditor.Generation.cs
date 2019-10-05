using Shiroi.Unity.Pathfinding2D.Runtime;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Editor {
    public partial class NavMesh2DEditor {
        public const float kDefaultBoxcastSize = 0.9F;
        public Vector2 boxCastSize = new Vector2(kDefaultBoxcastSize, kDefaultBoxcastSize);

        public void GenerateNodes(NavMesh2D navmesh) {
            var min = navmesh.Min;
            var max = navmesh.Max;
            var nodes = new Node[navmesh.Area];
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    var index = navmesh.IndexOfUnsafe(x, y);
                    Node.NodeFlags flags = 0;
                    if (Test(navmesh, x, y)) {
                        flags |= Node.NodeFlags.Solid;
                    }
                    else {
                        bool supported = false;
                        if (Test(navmesh, x, y - 1)) {
                            flags |= Node.NodeFlags.Supported;
                            supported = true;
                        }

                        if (Test(navmesh, x - 1, y)) {
                            flags |= Node.NodeFlags.LeftWall;
                        }
                        else {
                            if (supported && !Test(navmesh, x - 1, y - 1)) {
                                flags |= Node.NodeFlags.LeftEdge;
                            }
                        }

                        if (Test(navmesh, x + 1, y)) {
                            flags |= Node.NodeFlags.RightWall;
                        }
                        else {
                            if (supported && !Test(navmesh, x + 1, y - 1)) {
                                flags |= Node.NodeFlags.RightEdge;
                            }
                        }
                    }

                    nodes[index] = new Node {
                        flags = flags
                    };
                }
            }

            navmesh.ImportNodes(nodes);
        }

        private bool Test(NavMesh2D navmesh, int x, int y) {
            var position = navmesh.grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            var found = Physics2D.OverlapBox(position, boxCastSize, 0, navmesh.worldMask);
            return found;
        }
    }
}