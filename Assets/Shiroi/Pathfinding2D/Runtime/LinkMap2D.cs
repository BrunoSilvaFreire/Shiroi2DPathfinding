using UnityEngine;

namespace Shiroi.Pathfinding2D.Runtime {
    public abstract class LinkMap2D<L, G> : MonoBehaviour {
        public NavMesh2D<G> navMesh;
        public L[] links;

        public abstract L Generate(
            int x,
            int y
        );
    }
}