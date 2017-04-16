namespace Shiroi.Unity.Pathfinding2D {
    public class Node {
        public Node(MapPosition position) {
            Position = position;
        }

        public MapPosition Position {
            get;
            private set;
        }
        public enum NodeType {
            Blocked,
            LeftEdge,
            RightEdge,
            Solo,
            Platform
        }
    }
}