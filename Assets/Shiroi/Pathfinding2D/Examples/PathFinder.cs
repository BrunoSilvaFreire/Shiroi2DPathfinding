using System.Collections.Generic;
using System.Linq;
using Shiroi.Pathfinding2D.Kuroi;
using Shiroi.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Examples {
    public class PathFinder : MonoBehaviour {
        public Vector2Int origin, destination;
        public List<uint> path;
        public KuroiLinkMap linkMap;

        public void SwapPoints() {
            var t = origin;
            origin = destination;
            destination = t;
        }

        public void FindPath() {
            var navmesh = linkMap.NavMesh;
            path = AStar.CalculatePath(
                navmesh.IndexOf(origin.x, origin.y),
                navmesh.IndexOf(destination.x, destination.y),
                linkMap
            );
        }


        private void OnDrawGizmos() {
            var navmesh = linkMap.NavMesh;
            if (navmesh == null || navmesh.grid == null) {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(navmesh.grid.GetCellCenterWorld((Vector3Int) origin), 1F);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(navmesh.grid.GetCellCenterWorld((Vector3Int) destination), 1);
            Gizmos.color = Color.yellow;
            if (path == null) {
                return;
            }

            var color = new Color(1f, 0.67f, 0f);
            for (int i = 0; i < path.Count - 1; i++) {
                var oPos = navmesh.grid.CellToWorld((Vector3Int) navmesh.PositionOf(path[i + 1]));
                var link = FindLinkFromTo(path[i], path[i + 1]);

                Gizmos.color = color;

                if (link is KuroiLinkMap.LinkNode.GravitationalLink g) {
                    var points = g.Path;
                    for (var j = 0; j < points.Length - 1; j++) {
                        var a = points[j];
                        var b = points[j + 1];
                        Gizmos.DrawLine(
                            a,
                            b
                        );
                    }

                    continue;
                }

                Gizmos.DrawLine(
                    oPos,
                    navmesh.grid.GetCellCenterWorld((Vector3Int) navmesh.PositionOf(path[i]))
                );
            }
        }

        private ILink FindLinkFromTo(uint from, uint to) {
            return linkMap.nodes[from].Links.FirstOrDefault(
                link => link.Destination == to
            );
        }
    }
}