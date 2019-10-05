using Shiroi.Unity.Pathfinding2D.Runtime;
using UnityEditor;

namespace Shiroi.Unity.Pathfinding2D.Editor {
    public partial class LinkMap2DEditor {
        private void OnSceneGUI() {
            var l = (LinkMap2D) target;
            var navmesh = l.navMesh;
            if (navmesh == null) {
                return;
            }

            var min = navmesh.Min;
            var max = navmesh.Max;
            var links = l.links;
            if (links == null) {
                return;
            }
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    var index = navmesh.IndexOfUnsafe(x, y);
                    var link = links[index];
                    foreach (var directLink in link.directLinks) {
                        var otherPos = navmesh.PositionOf(directLink.destination);
                        Handles.DrawDottedLine(
                            navmesh.WorldCenter(x, y),
                            navmesh.WorldCenter(otherPos.x, otherPos.y),
                            NavMesh2DEditor.kBoundariesWidth
                        );
                    }
                }
            }
        }
    }
}