using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public partial class LinkMap2DEditor<L, G, N> {
        public int linkRadius;
        public bool drawDirect;
        public bool drawGravitational;

        /*private void OnSceneGUI() {
            var linkmap = (LinkMap2D<L, G>) target;
            var navmesh = linkmap.navMesh;
            if (navmesh == null) {
                return;
            }

            var navigationNodes = linkmap.links;
            if (navigationNodes == null || navigationNodes.Length != navmesh.Area) {
                return;
            }

            var grid = navmesh.grid;
            var gridSize = grid.cellSize;
            var width = navmesh.Width;
            var mousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            var mouseCell = grid.WorldToCell(mousePos);

            for (var x = -linkRadius; x <= linkRadius; x++) {
                for (var y = -linkRadius; y <= linkRadius; y++) {
                    var gridPos = new Vector3Int(x, y, 0) + mouseCell;
                    if (navmesh.IsOutOfBounds((Vector2Int) gridPos)) {
                        continue;
                    }

                    var index = navmesh.IndexOf(gridPos.x, gridPos.y);
                    var origin = grid.GetCellCenterWorld(gridPos);
                    var node = navigationNodes[index];
                    var gNode = navmesh.Nodes[index];
                    if (node.directLinks == null) {
                        continue;
                    }

                    if (drawDirect) {
                        foreach (var link in node.directLinks) {
                            var nI = (Vector3Int) navmesh.PositionOf(link.Destination);
                            var pos = grid.GetCellCenterWorld(nI);
                            var neightboor = navmesh.Nodes[link.Destination];
                            var dir = pos - origin;
                            var color = new Color((dir.x + 1) / 2, (dir.y + 1) / 2, 1);
                            var n = dir.normalized;

                            EditorX.ForHandles(origin, dir, color);
                        }
                    }

                    if (drawGravitational) {
                        foreach (var link in node.gravitationalLinks) {
                            var nI = (Vector3Int) navmesh.PositionOf(link.Destination);
                            var pos = grid.GetCellCenterWorld(nI);
                            float size = 4;
                            var p = link.Path;


                            for (int i = 0; i < p.Length - 1; i++) {
                                var current = p[i];
                                var next = p[i + 1];
                                var dir = next - current;
                                dir.Normalize();
                                var color = new Color((dir.x + 1) / 2, (dir.y + 1) / 2, 0);
                                var prog = (float) i / p.Length;
                                color.b = prog;
                                color.a = 0.5F + (prog / 2);
                                Handles.color = color;
                                Handles.DrawDottedLine(current, next, size);
                            }
                        }
                    }
                }
            }

            var radius = new Vector2Int(linkRadius, linkRadius);
            Handles.color = Color.red;
            EditorX.DrawBoundaries(
                (Vector2Int) mouseCell - radius,
                (Vector2Int) mouseCell + radius, grid
            );
        }*/
    }
}