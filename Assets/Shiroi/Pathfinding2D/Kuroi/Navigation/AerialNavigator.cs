/*using Datenshi.Entities;
using Datenshi.Entities.Input;
using Lunari.Tsuki;
using UnityEngine;

namespace Datenshi.AI.Pathfinding.Navigator {
    public class AerialNavigator : Navigator {
        protected override void Navigate(Entity owner, NavigationTrip trip) {
            if (trip.GetPath(out var path)) {
                var i = owner.GetTrait<EntityInput>();
                if (path.IsEmpty()) {
                    return;
                }
                var target = path[0];
                var oPos = navmesh.IndexOf(owner.transform);
                if (oPos == target) {
                    path.RemoveAt(0);
                    i.horizontal = 0;
                    i.vertical = 0;
                    return;
                }

                var tPos = (Vector3Int) navmesh.PositionOf(target);
                var pos = navmesh.grid.GetCellCenterWorld(tPos);
                var delta = pos - transform.position;
                delta.Normalize();
                i.horizontal = delta.x;
                i.vertical = delta.y;
            }
        }

        public override bool CanTransverse(ILink link) {
            return !(link is GravitationalLink);
        }
    }
}*/