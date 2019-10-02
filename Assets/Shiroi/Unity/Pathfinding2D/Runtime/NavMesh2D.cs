using System;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Runtime {
    [Serializable]
    public struct Node {
        public NodeFlags flags;

        [Flags]
        public enum NodeFlags {
            Solid = 1 << 0,
            LeftWall = 1 << 1,
            RightWall = 1 << 2,
            LeftEdge = 1 << 3,
            RightEdge = 1 << 4
        }
    }

    public class NavMesh2D : MonoBehaviour {
        [SerializeField, HideInInspector]
        private Node[] nodes;

        public Grid grid;
        public Vector2Int min, max;
    }
}