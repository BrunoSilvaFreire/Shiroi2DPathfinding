using System;
using JetBrains.Annotations;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D {
    [Serializable]
    public class Node {
        public Node(MapPosition position, NodeType type) {
            Position = position;
            Debug.Log(Position);
            Type = type;
        }

        [Show]
        public NodeType Type {
            get;
            [Hide] private set;
        }

        [Show]
        public bool Walkable {
            get { return Type != NodeType.Blocked; }
        }

        [NotNull]
        public MapPosition Position {
            get;
            private set;
        }

        [CanBeNull]
        public Platform Platform {
            get;
            set;
        }

        public int X {
            get {
                return Position.X;
            }
        }

        public int Y {
            get {
                return Position.Y;
            }
        }

        public Vector2 PositionVec {
            get { return new Vector2(X + .5f, Y + .5F); }
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
    }
}