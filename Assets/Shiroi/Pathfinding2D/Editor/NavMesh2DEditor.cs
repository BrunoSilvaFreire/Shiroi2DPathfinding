using Shiroi.Pathfinding2D.Editor.Validation;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public partial class NavMesh2DEditor<G> : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var navmesh = target as NavMesh2D<G>;
            if (navmesh == null) {
                return;
            }

            using (new EditorGroupScope("NavMesh")) {
                navmesh.grid = (Grid) EditorGUILayout.ObjectField("Grid", navmesh.grid, typeof(Grid), true);
                var nMin = EditorGUILayout.Vector2IntField("Min", navmesh.Min);
                var nMax = EditorGUILayout.Vector2IntField("Max", navmesh.Max);
                navmesh.SetMinMax(nMin, nMax);
                navmesh.worldMask = UnityX.LayerMaskField("World Mask", navmesh.worldMask);
                OnNavMeshGUI();
                if (GUILayout.Button("Generate Mesh")) {
                    GenerateNodes(navmesh);
                }
            }

            using (new EditorGroupScope("Info")) {
                GUI.enabled = false;
                EditorGUILayout.Vector2Field("Size", new Vector2(navmesh.Width, navmesh.Height));
                EditorGUILayout.LabelField("Area", navmesh.Area.ToString());
                GUI.enabled = true;
            }

            using (new EditorGroupScope("Editor")) {
                drawEmpty = EditorGUILayout.Toggle("Draw Empty", drawEmpty);
                drawSolids = EditorGUILayout.Toggle("Draw Solids", drawSolids);
                drawMouse = EditorGUILayout.Toggle("Draw Over Mouse", drawMouse);
                Validators.ValidateInEditor(
                    navmesh,
                    Validators<G>.HasNodes,
                    Validators<G>.NodesMatchGeometry
                );
            }
        }

        public virtual void OnNavMeshGUI() { }
    }
}