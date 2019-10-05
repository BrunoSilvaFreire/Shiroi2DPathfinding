using System;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Runtime {
    public class LinkMap2D : MonoBehaviour {
        [Serializable]
        public struct LinkNode {
            [Serializable]
            public struct DirectLink {
                public uint destination;
            }

            public DirectLink[] directLinks;
        }

        public NavMesh2D navMesh;
        public LinkNode[] links;
    }
}