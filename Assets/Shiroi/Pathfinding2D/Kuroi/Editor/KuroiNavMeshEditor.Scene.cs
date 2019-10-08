using System;
using Shiroi.Pathfinding2D.Editor;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Kuroi.Editor {
    public partial class KuroiNavMeshEditor {
        private void OnSceneGUI() {
            var navmesh = (KuroiNavMesh) target;
            DoDefaultSceneGUI(navmesh);
            if (navmesh.grid == null) {
                return;
            }

            DrawNodes(navmesh);
            DrawLabels(navmesh);
        }

        private void DrawNodes(KuroiNavMesh navmesh) {
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
                        //color.r = 1;
                        outlineColor.r = 1;
                    }

                    if (node.IsRightEdge()) {
                        //color.b = 1;
                        outlineColor.b = 1;
                    }

                    UnityX.DrawCell(x, y, navmesh.grid, color, outlineColor);
                }
            }
        }

        private void DrawLabels(KuroiNavMesh navmesh) {
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
        }
    }
}