using JetBrains.Annotations;

namespace Shiroi.Unity.Pathfinding2D {
    public class Node {
        public Node(MapPosition position, NodeType type) {
            Position = position;
            Type = type;
        }

        public NodeType Type {
            get;
            private set;
        }

        public bool Walkable {
            get {
                return Type != NodeType.Blocked;
            }
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