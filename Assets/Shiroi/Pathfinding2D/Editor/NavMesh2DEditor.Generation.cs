using Shiroi.Pathfinding2D.Runtime;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public partial class NavMesh2DEditor<G> {
        public const float kDefaultBoxcastSize = 0.9F;

        public void GenerateNodes(NavMesh2D<G> navmesh) {
            var min = navmesh.Min;
            var max = navmesh.Max;
            var nodes = new G[navmesh.Area];
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    var index = navmesh.IndexOfUnsafe(x, y);
                    nodes[index] = navmesh.GenerateNode(x, y);

                }
            }

            navmesh.ImportNodes(nodes);
        }

    }
}