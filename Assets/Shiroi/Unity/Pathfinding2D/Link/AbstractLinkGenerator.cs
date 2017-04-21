using System.Collections.Generic;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public abstract class AbstractLinkGenerator : ILinkGenerator {
        public LinkMap LinkMap;
        public TileMap TileMap;

        protected AbstractLinkGenerator(LinkMap linkMap) {
            LinkMap = linkMap;
            this.TileMap = linkMap.TileMap;
        }

        public abstract IEnumerable<ILink> Generate(Node node);
    }
}