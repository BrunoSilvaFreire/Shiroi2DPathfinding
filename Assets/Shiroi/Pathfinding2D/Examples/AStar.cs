using System.Collections.Generic;
using Lunari.Tsuki;
using Shiroi.Pathfinding2D.Kuroi;
using Shiroi.Pathfinding2D.Runtime;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Examples {
    public class AStar {
        public static List<uint> CalculatePath(
            uint @from,
            uint to,
            KuroiLinkMap linkMap
        ) {
            var navMesh = linkMap.NavMesh;
            // The set of nodes already evaluated.
            var closedSet = new List<uint>();
            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new List<uint> {
                from
            };
            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<uint, uint>();
            var gScore = new Dictionary<uint, float> {
                [from] = 0.0f
            };
            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<uint, float>();

            fScore[from] = Distance(from, to, navMesh);
            while (!openSet.IsEmpty()) {
                // the node in openSet having the lowest fScore[] value
                var current = openSet.MinBy(node => fScore.GetOrPut(node, () => float.PositiveInfinity));
                if (current == to) {
                    return Reconstruct(cameFrom, current, navMesh);
                }

                openSet.Remove(current);
                closedSet.Add(current);
                foreach (var link in linkMap.nodes[current].Links) {
                    if (!CanTransverse(navMesh, link)) {
                        continue;
                    }

                    var neighbor = link.Destination;
                    if (closedSet.Contains(neighbor)) {
                        // Ignore the neighbor which is already evaluated.
                        continue;
                    }

                    var currentGScore = gScore.GetOrPut(current, () => float.PositiveInfinity);
                    var neightborGScore = gScore.GetOrPut(neighbor, () => float.PositiveInfinity);
                    var linkDistance = link.CalculateCost(linkMap);
                    // The distance from start to a neighbor
                    var tentativeGScore = currentGScore + linkDistance;
                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    } else if (tentativeGScore >= neightborGScore) {
                        // This is not a better path.
                        continue;
                    }

                    // This path is the best until now. Record it!
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Distance(neighbor, to, navMesh);
                }
            }

            Debug.LogWarning($"Unable to find path from {@from} to {to}");
            return null;
        }

        public static bool CanTransverse(NavMesh2D<KuroiNavMesh.GeometryNode> navmesh, ILink link) {
            if (link is DirectLink direct) {
                return navmesh.Nodes[direct.Destination].IsSupported();
            }

            return true;
        }

        private static List<uint> Reconstruct(IDictionary<uint, uint> cameFrom, uint current,
            NavMesh2D<KuroiNavMesh.GeometryNode> navmesh) {
            if (!cameFrom.ContainsKey(current)) {
                return null;
            }

            var currentCameFrom = cameFrom[current];
            var totalPath = new List<uint> {
                currentCameFrom
            };
            while (cameFrom.ContainsKey(current)) {
                if (!cameFrom.TryGetValue(current, out var link)) {
                    Debug.Log("Couldn't find value for " + current + "@ " + cameFrom);
                    return totalPath;
                }

                current = link;
                totalPath.Insert(0, link);
            }

            return totalPath;
        }

        private static float Distance(uint @from, uint to, NavMesh2D<KuroiNavMesh.GeometryNode> navMesh) {
            return Vector2.Distance(
                navMesh.grid.GetCellCenterWorld((Vector3Int) navMesh.PositionOf(from)),
                navMesh.grid.GetCellCenterWorld((Vector3Int) navMesh.PositionOf(to))
            );
        }
    }
}