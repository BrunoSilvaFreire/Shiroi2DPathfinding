using JetBrains.Annotations;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public partial class NavMesh2DEditor<G> {
        public bool drawSolids;
        public bool drawEmpty;
        public bool drawMouse;

        private void OnSceneGUI() {
            var navmesh = target as NavMesh2D<G>;
            if (navmesh == null) {
                return;
            }

            var grid = navmesh.grid;
            if (grid == null) {
                return;
            }

            var min = navmesh.Min;
            var max = navmesh.Max;
            DoPositionHandle(grid, ref min);
            DoPositionHandle(grid, ref max);
            navmesh.Min = min;
            navmesh.Max = max;
            //DrawNodes(navmesh);
            DrawBoundaries(navmesh);
            //DrawLabels(navmesh);
        }

        /*private void DrawNodes(NavMesh2D<G> navmesh) {
            var nodes = navmesh.Nodes;
            if (nodes == null || nodes.Length != navmesh.Area) {
                return;
            }

            var min = navmesh.Min;
            var max = navmesh.Max;

            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    var index = navmesh.IndexOfUnsafe(x, y);
                    var node = nodes[index];
                    if (!drawSolids && node.IsSolid()) {
                        continue;
                    }

                    if (!drawEmpty && node.IsEmpty()) {
                        continue;
                    }

                    Color color = Color.black;
                    color.a = node.IsSolid() ? 0.9F : 0.5f;
                    Color outlineColor = Color.black;
                    if (node.IsSupported()) {
                        color.g = 1;
                    }

                    if (node.IsLeftWall()) {
                        color.r = 1;
                    }

                    if (node.IsRightWall()) {
                        color.b = 1;
                    }

                    if (node.IsLeftEdge()) {
                        color.r = 1;
                        outlineColor.r = 1;
                    }

                    if (node.IsRightEdge()) {
                        color.b = 1;
                        outlineColor.b = 1;
                    }

                    var pos = navmesh.WorldCenter(x, y);
                    var cellSize = navmesh.grid.cellSize;
                    var width = cellSize.x;
                    var height = cellSize.y;
                    Handles.DrawSolidRectangleWithOutline(
                        new Rect(
                            pos.x - width / 2,
                            pos.y - height / 2,
                            width,
                            height
                        ),
                        color,
                        outlineColor
                    );
                }
            }
        }

        private void DrawLabels(NavMesh2D navmesh) {
            var min = navmesh.MinWorld;
            var max = navmesh.MaxWorld;
            var nodes = navmesh.Nodes;
            if (nodes == null || nodes.Length != navmesh.Area) {
                return;
            }

            Handles.Label(
                min,
                $"Min: {min}",
                EditorStyles.helpBox
            );
            Handles.Label(
                max,
                $"Max: {max}",
                EditorStyles.helpBox
            );
            var mousePos = Event.current.mousePosition;
            mousePos.y = Camera.current.pixelHeight - mousePos.y;

            var worldPosition = Camera.current.ScreenToWorldPoint(mousePos);
            worldPosition.z = 0;
            var cell = navmesh.grid.WorldToCell(worldPosition);
            if (drawMouse && !navmesh.IsOutOfBounds(cell.x, cell.y)) {
                var node = navmesh.NodeUnsafe(cell.x, cell.y);
                var index = navmesh.IndexOfUnsafe(cell.x, cell.y);
                Handles.Label(
                    navmesh.WorldCenter(cell.x, cell.y),
                    $"Node #{index} {cell}:\n" +
                    $"Solid? {node.IsSolid()}\n" +
                    $"Supported? {node.IsSupported()}\n" +
                    $"LeftWall? {node.IsLeftWall()}\n" +
                    $"RightWall? {node.IsRightWall()}\n" +
                    $"LeftEdge? {node.IsLeftEdge()}\n" +
                    $"RightEdge? {node.IsRightEdge()}",
                    EditorStyles.helpBox
                );
            }
        }*/

        private static void DrawBoundaries(NavMesh2D<G> navmesh) {
            var min = navmesh.Min;
            var max = navmesh.Max;
            EditorX.DrawBoundaries(min, max, navmesh.grid);
        }

        private static void DoPositionHandle(
            [NotNull] Grid grid,
            ref Vector2Int result
        ) {
            var current = grid.GetCellCenterWorld((Vector3Int) result);
            current = Handles.PositionHandle(current, Quaternion.identity);
            result = (Vector2Int) grid.WorldToCell(current);
        }
    }
}