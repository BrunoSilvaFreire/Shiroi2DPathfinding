using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public class LinearLink : ILink {
        public LinearLink(Node from, Node to) {
            this.From = from;
            this.To = to;
            this.Distance = Vector2.Distance(from.Position, to.Position);
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
            get {
                return LinkType.Run;
            }
        }

        public void DrawGizmos() {
            Gizmos.DrawLine(From.Position, To.Position);
        }
    }
}