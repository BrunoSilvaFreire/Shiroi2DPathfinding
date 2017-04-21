using JetBrains.Annotations;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D {
    public class Node {
        public Node(MapPosition position, NodeType type) {
            Position = position;
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
            get { return Position.X; }
        }

        public int Y {
            get { return Position.Y; }
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