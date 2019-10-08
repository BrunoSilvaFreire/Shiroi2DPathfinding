using UnityEditor;
using UnityEngine;


namespace Shiroi.Pathfinding2D.Examples.Editor {
    [CustomEditor(typeof(PathFinder))]
    public class PathFinderEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            using (new EditorGUILayout.HorizontalScope()) {
                var p = (PathFinder) target;
                if (GUILayout.Button("Find Path")) {
                    p.FindPath();
                }

                if (GUILayout.Button("Swap")) {
                    p.SwapPoints();
                }
            }
        }
    }
}