namespace Shiroi.Unity.Pathfinding2D.Link {
    public interface ILink {
        Node From {
            get;
        }

        Node To {
            get;
        }

        float Distance {
            get;
        }

        LinkType Type {
            get;
        }

        void DrawGizmos();
    }

    public enum LinkType {
        Run,
        Fall,
        Jump
    }
}