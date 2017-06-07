using System;
using JetBrains.Annotations;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D {
    [Serializable]
    public class Node {
        [SerializeField, Hide]
        private NodeType type;

        [SerializeField, Hide]
        private MapPosition position;

        public Node(MapPosition position, NodeType type) {
            this.position = position;
            this.type = type;
        }

        public NodeType Type {
            get { return type; }
        }

        public bool Walkable {
            get { return Type != NodeType.Blocked; }
        }

        public MapPosition Position {
            get { return position; }
        }
        public int X {
            get { return Position.X; }
        }

        public int Y {
            get { return Position.Y; }
        }

        public Vector2 PositionVec {
            get { return new Vector2(X + .5f, Y + .5F); }
        }

        public bool Empty {
            get { return Type == NodeType.Empty; }
        }

        public override string ToString() {
            return string.Format("Node{{Type: {0}, X: {1}, Y: {2}}}", Type, X, Y);
        }

        [Serializable]
        public enum NodeType {
            Empty,
            Blocked,
            LeftEdge,
            RightEdge,
            Solo,
            Platform,
        }

        public float Distance(Node neighbor) {
            return Position.Distance(neighbor.Position);
        }
    }
}