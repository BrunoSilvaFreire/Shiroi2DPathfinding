/*using System.Linq;
using Datenshi.Entities;

namespace Datenshi.AI.Pathfinding.Navigator {
    public class TerrestrialNavigator : Navigator {
        protected override void Navigate(Entity owner, NavigationTrip trip) {
            if (trip.GetPath(out var path)) {
                var current = path.First();
            }
        }

        public override bool CanTransverse(ILink link) {
            if (link is DirectLink direct) {
                return navmesh.geometryNodes[direct.destination].IsPlatform();
            }

            return true;
        }
    }
}*/