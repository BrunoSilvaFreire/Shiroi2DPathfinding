using System.Collections.Generic;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public class RunLinkGenerator : AbstractLinkGenerator {
        public RunLinkGenerator(LinkMap linkMap) : base(linkMap) {
        }

        public override IEnumerable<ILink> Generate(Node node) {
            var links = new List<ILink>();

            var leftNode = TileMap.GetNode(node, Direction.Left);
            if (leftNode != null && leftNode.Walkable) {
                links.Add(new LinearLink(node, leftNode));
            }

            var rightNode = TileMap.GetNode(node, Direction.Right);
            if (rightNode != null && rightNode.Walkable) {
                links.Add(new LinearLink(node, rightNode));
            }

            return links;
        }
    }
}