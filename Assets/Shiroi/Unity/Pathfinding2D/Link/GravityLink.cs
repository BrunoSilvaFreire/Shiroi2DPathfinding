namespace Shiroi.Unity.Pathfinding2D.Link {
    public class GravityLink : ILink{
        public GravityLink(Node @from, Node to, LinkType type) {
            From = @from;
            To = to;
            Type = type;
        }

        public Node From {
            get;
            private set;
        }

        public Node To {
            get;
            private set;
        }

        public float Distance {
            get;
            private set;
        }

        public LinkType Type {
            get;
            private set;
        }

        public void DrawGizmos() {

        }
    }
}