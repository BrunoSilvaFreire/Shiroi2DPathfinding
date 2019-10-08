using UnityEngine;

namespace Shiroi.Pathfinding2D.Runtime {
    public abstract class LinkMap2D<L, G> : MonoBehaviour {
        [SerializeField]
        private Object serializedNavMesh;

        public NavMesh2D<G> NavMesh {
            get => serializedNavMesh as NavMesh2D<G>;
            set => serializedNavMesh = value;
        }

        public L[] nodes;

        public abstract L Generate(
            int x,
            int y
        );
    }
}