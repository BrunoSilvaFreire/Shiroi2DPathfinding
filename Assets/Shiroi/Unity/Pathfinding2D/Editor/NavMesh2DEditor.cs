using JetBrains.Annotations;
using Shiroi.Unity.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Editor {
    [CustomEditor(typeof(NavMesh2D))]
    public partial class NavMesh2DEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var navmesh = target as NavMesh2D;
            if (navmesh == null) {
                return;
            }

            navmesh.grid = (Grid) EditorGUILayout.ObjectField("Grid", navmesh.grid, typeof(Grid), true);

            navmesh.worldMask = UnityX.LayerMaskField("World Mask", navmesh.worldMask);
            if (GUILayout.Button("Generate Mesh")) {
                GenerateNodes(navmesh);
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                GUI.enabled = false;
                EditorGUILayout.PrefixLabel("NavMesh info");
                EditorGUILayout.Vector2Field("Size", new Vector2(navmesh.Width, navmesh.Height));
                EditorGUILayout.IntField("Area", (int) navmesh.Area);
                GUI.enabled = true;
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                EditorGUILayout.PrefixLabel("Editor");
                boxCastSize = EditorGUILayout.Vector2Field("BoxCastSize", boxCastSize);
            }
        }
    }
}