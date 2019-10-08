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
    }
}