using Shiroi.Pathfinding2D.Editor;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Kuroi.Editor {
    [CustomEditor(typeof(KuroiLinkMap))]
    public class
        KuroiLinkMapEditor : LinkMap2DEditor<KuroiLinkMap.LinkNode, KuroiNavMesh.GeometryNode, KuroiNavMesh> {
        public int linkRadius;
        public bool drawDirect;
        public bool drawGravitational;
        public bool drawHeat;
        private uint? withMostConnectionIndex;

        public uint WithMostConnectionIndex {
            get {
                if (withMostConnectionIndex == null) {
                    withMostConnectionIndex = FindHighest();
                }

                return withMostConnectionIndex.Value;
            }
        }

        private uint FindHighest() {
            var l = (KuroiLinkMap) target;
            var first = l.nodes[0];
            uint highest = 0;
            uint highestScore = (uint) (first.directLinks.Length + first.gravitationalLinks.Length);
            for (uint i = 0; i < l.nodes.Length; i++) {
                var link = l.nodes[i];
                uint c = (uint) (link.directLinks.Length + link.gravitationalLinks.Length);
                if (c > highestScore) {
                    highest = i;
                    highestScore = c;
                }
            }

            return highestScore;
        }

        protected override void OnLinkMapGUI() {
            var linkMap = (KuroiLinkMap) target;
            linkMap.timeStep = EditorGUILayout.Slider("Time Step", linkMap.timeStep, 0.001F, 100);
            linkMap.jumpCount = (uint) EditorGUILayout.IntField("Jump Count", (int) linkMap.jumpCount);
            linkMap.hitBoxSize = EditorGUILayout.Vector2Field("Hitbox Size", linkMap.hitBoxSize);
            linkMap.maxForce = EditorGUILayout.Vector2Field("Max Force", linkMap.maxForce);
        }

        protected override void OnEditorGUI() {
            linkRadius = EditorGUILayout.IntField("Preview Radius", linkRadius);
            drawDirect = EditorGUILayout.Toggle("Draw Direct", drawDirect);
            drawGravitational = EditorGUILayout.Toggle("Draw Gravitational", drawGravitational);
            drawHeat = EditorGUILayout.Toggle("Draw Heat", drawHeat);
        }

        private void OnSceneGUI() {
            var linkmap = (KuroiLinkMap) target;
            var navmesh = linkmap.NavMesh;
            if (navmesh == null) {
                return;
            }

            var navigationNodes = linkmap.nodes;
            if (navigationNodes == null || navigationNodes.Length != navmesh.Area) {
                return;
            }

            var grid = navmesh.grid;
            var mousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            var mouseCell = grid.WorldToCell(mousePos);
            if (drawHeat) {
                var nodes = navmesh.Nodes;
                if (nodes == null || nodes.Length != navmesh.Area) {
                    return;
                }

                var highest = WithMostConnectionIndex;

                var min = navmesh.Min;
                var max = navmesh.Max;

                for (var x = min.x; x <= max.x; x++) {
                    for (var y = min.y; y <= max.y; y++) {
                        var index = navmesh.IndexOfUnsafe(x, y);
                        var other = navmesh.Nodes[index];

                        if (other.IsSolid()) {
                            continue;
                        }

                        var node = navigationNodes[index];
                        var c = node.directLinks.Length + node.gravitationalLinks.Length;
                        if (c == 0) {
                            continue;
                        }

                        var color = Color.black;
                        color.r = ((float) c / highest);
                        color.a = 1;
                        UnityX.DrawCell(x, y, navmesh.grid, color, Color.clear);
                    }
                }
            }

            /*Handles.DrawSolidRectangleWithOutline(
                new Rect(
                    (Vector2) mousePos - linkmap.hitBoxSize / 2, linkmap.hitBoxSize),
                Color.red,
                Color.clear
            );*/
            for (var x = -linkRadius; x <= linkRadius; x++) {
                for (var y = -linkRadius; y <= linkRadius; y++) {
                    var gridPos = new Vector3Int(x, y, 0) + mouseCell;


                    var index = navmesh.IndexOfUnsafe(gridPos.x, gridPos.y);
                    if (index > navmesh.Nodes.Length) {
                        continue;
                    }

                    var origin = grid.GetCellCenterWorld(gridPos);
                    var node = navigationNodes[index];

                    if (drawDirect) {
                        foreach (var link in node.directLinks) {
                            var nI = (Vector3Int) navmesh.PositionOf(link.Destination);
                            var pos = grid.GetCellCenterWorld(nI);
                            var neightboor = navmesh.Nodes[link.Destination];
                            var dir = pos - origin;
                            var n = dir.normalized;
                            var color = new Color((n.x + 1) / 2, (n.y + 1) / 2, 1);
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
                                Handles.DrawLine(current, next);
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
        }
    }
}