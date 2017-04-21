using System.Collections.Generic;
using Shiroi.Unity.Pathfinding2D.Util;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public class GravityLink : ILink {
        public GravityLink(Node from, Vector2 speed, float timeIncrementation, TileMap tileMap, IGroundEntity entity,
            LinkType type) {
            From = from;
            Type = type;
            Node end;
            Path = GravityUtil.CalculatePath(from.Position, speed, timeIncrementation, tileMap, entity, out end);
            To = end;
            Distance = GravityUtil.CalculateDistance(Path);
        }

        public List<Vector2> Path {
            get;
            private set;
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
            GravityUtil.DrawGizmos(Path);
        }
    }
}