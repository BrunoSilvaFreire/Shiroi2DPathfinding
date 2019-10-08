using Shiroi.Pathfinding2D.Editor;
using UnityEditor;

namespace Shiroi.Pathfinding2D.Kuroi.Editor {
    [CustomEditor(typeof(KuroiLinkMap))]
    public class
        KuroiLinkMapEditor : LinkMap2DEditor<KuroiLinkMap.LinkNode, KuroiNavMesh.GeometryNode, KuroiNavMesh> { }
}