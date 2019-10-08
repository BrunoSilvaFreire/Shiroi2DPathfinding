using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public static class UnityX {
        public static LayerMask LayerMaskField(string label, LayerMask current) {
            var layerNames = new List<string>();
            for (var i = 0; i < 32; i++) {
                var layerN = LayerMask.LayerToName(i); //get the name of the layer
                layerNames.Add(layerN);
            }

            return EditorGUILayout.MaskField(label, current, layerNames.ToArray());
        }

        public const float kBoundariesWidth = 2F;

        public static void DrawCell(
            int x, int y, Grid grid, Color first, Color second) {
            var pos = grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            var cellSize = grid.cellSize;
            var width = cellSize.x;
            var height = cellSize.y;
            Handles.DrawSolidRectangleWithOutline(
                new Rect(
                    pos.x - width / 2,
                    pos.y - height / 2,
                    width,
                    height
                ),
                first,
                second
            );
        }
        
    }
}