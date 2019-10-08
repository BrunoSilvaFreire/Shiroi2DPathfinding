using Shiroi.Pathfinding2D.Runtime;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public partial class NavMesh2DEditor<G> {
        public const float kDefaultBoxcastSize = 0.9F;
        public Vector2 boxCastSize;

        /*public void GenerateNodes(NavMesh2D navmesh) {
            var min = navmesh.Min;
            var max = navmesh.Max;
            var nodes = new NavMesh2D.GeometryNode[navmesh.Area];
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    var index = navmesh.IndexOfUnsafe(x, y);
                    NavMesh2D.GeometryNode.NodeFlags flags = 0;
                    if (Test(navmesh, x, y)) {
                        flags |= NavMesh2D.GeometryNode.NodeFlags.Solid;
                    }
                    else {
                        bool supported = false;
                        if (Test(navmesh, x, y - 1)) {
                            flags |= NavMesh2D.GeometryNode.NodeFlags.Supported;
                            supported = true;
                        }

                        if (Test(navmesh, x - 1, y)) {
                            flags |= NavMesh2D.GeometryNode.NodeFlags.LeftWall;
                        }
                        else {
                            if (supported && !Test(navmesh, x - 1, y - 1)) {
                                flags |= NavMesh2D.GeometryNode.NodeFlags.LeftEdge;
                            }
                        }

                        if (Test(navmesh, x + 1, y)) {
                            flags |= NavMesh2D.GeometryNode.NodeFlags.RightWall;
                        }
                        else {
                            if (supported && !Test(navmesh, x + 1, y - 1)) {
                                flags |= NavMesh2D.GeometryNode.NodeFlags.RightEdge;
                            }
                        }
                    }

                    nodes[index] = new NavMesh2D.GeometryNode {
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
        }*/
    }
}