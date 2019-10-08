using Shiroi.Pathfinding2D.Editor;
using UnityEditor;

namespace Shiroi.Pathfinding2D.Kuroi.Editor {
    [CustomEditor(typeof(KuroiNavMesh))]
    public partial class KuroiNavMeshEditor : NavMesh2DEditor<KuroiNavMesh.GeometryNode> {
        public override void OnNavMeshGUI() {
            var nav = (KuroiNavMesh) target;
            nav.boxCastSize = EditorGUILayout.Vector2Field("BoxCastSize", nav.boxCastSize);
        }
    }
}