using JetBrains.Annotations;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public partial class NavMesh2DEditor<G> {
        public bool drawSolids;
        public bool drawEmpty;
        public bool drawMouse;

        public static void DoDefaultSceneGUI(NavMesh2D<G> target) {
            var navmesh = target;
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
            DrawBoundaries(navmesh);
        }

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