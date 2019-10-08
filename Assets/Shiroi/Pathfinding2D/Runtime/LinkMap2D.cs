using UnityEngine;

namespace Shiroi.Pathfinding2D.Runtime {
    public class LinkMap2D<L, G> : MonoBehaviour {
        public NavMesh2D<G> navMesh;
        public L[] links;
    }
}