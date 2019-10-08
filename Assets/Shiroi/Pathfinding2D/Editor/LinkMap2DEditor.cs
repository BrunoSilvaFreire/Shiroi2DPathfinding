using Lunari.Tsuki.Scopes;
using Shiroi.Pathfinding2D.Editor.Validation;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public partial class LinkMap2DEditor<L, G, N> : UnityEditor.Editor where N : NavMesh2D<G> {
        public override void OnInspectorGUI() {
            var linkMap = target as LinkMap2D<L, G>;
            if (linkMap == null) {
                Debug.LogError("Expected LinkMap2D<L, G>, but found " + target);
                return;
            }

            using (new EditorGroupScope("Link Map")) {
                linkMap.NavMesh =
                    (NavMesh2D<G>) EditorGUILayout.ObjectField("Navmesh", linkMap.NavMesh, typeof(N), true);
                OnLinkMapGUI();
            }

            using (new EditorGroupScope("Info")) {
                var l = linkMap.nodes;
                if (l != null) {
                    EditorGUILayout.LabelField("Link count", l.Length.ToString());
                }
            }

            using (new EditorGroupScope("Editor")) {
                OnEditorGUI();
                using (new GUIEnabledScope(Validators<L, G>.HasNavMesh.Check(linkMap))) {
                    if (GUILayout.Button("Generate Links")) {
                        GenerateLinks(linkMap);
                    }
                }


                Validators.ValidateInEditor(
                    linkMap,
                    Validators<L, G>.HasNavMesh,
                    Validators<L, G>.LinksMatchGeometry
                );
            }
        }

        protected virtual void OnLinkMapGUI() { }

        public void GenerateLinks(LinkMap2D<L, G> linkMap) {
            var navmesh = linkMap.NavMesh;
            var min = navmesh.Min;
            var max = navmesh.Max;
            var nodes = new L[navmesh.Area];

            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    var index = linkMap.NavMesh.IndexOfUnsafe(x, y);
                    nodes[index] = linkMap.Generate(x, y);
                }
            }

            linkMap.nodes = nodes;
        }

        protected virtual void OnEditorGUI() { }
    }
}