using System.Collections.Generic;

namespace Shiroi.Unity.Pathfinding2D.Link {
    public interface ILinkGenerator {
        IEnumerable<ILink> Generate(Node node);
    }
}