using System;
using System.Collections.Generic;
using System.Linq;
using Shiroi.Unity.Pathfinding2D.Link;
using UnityEngine;
using UnityEngine.Experimental.Director;
using Vexe.Runtime.Extensions;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public static class AStarPathfinding {
        public static List<ILink> CalculatePath(Node from, Node to, LinkMap linkMap) {
            // The set of nodes already evaluated.
            var closedSet = new List<Node>();
            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new List<Node> {
                from
            };
            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<Node, ILink>();
            var gScore = new Dictionary<Node, float>();
            gScore[from] = 0.0f;
            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<Node, float>();
            fScore[from] = from.Position.Distance(to.Position);
            var current = from;
            while (!openSet.IsEmpty()) {
                // the node in openSet having the lowest fScore[] value
                current = openSet.MinBy(node => fScore.GetOrPut(node, float.PositiveInfinity));
                if (current == to) {
                    return Reconstruct(cameFrom, current);
                }
                openSet.Remove(current);
                closedSet.Add(current);
                var linkPoint = linkMap.GetLinkPoint(current);
                foreach (var link in linkPoint.Links) {
                    var neighbor = link.To;
                    if (closedSet.Contains(neighbor)) {
                        // Ignore the neighbor which is already evaluated.
                        continue;
                    }

                    var currentGScore = gScore.GetOrPut(current, float.PositiveInfinity);
                    var neightborGScore = gScore.GetOrPut(neighbor, float.PositiveInfinity);
                    var linkDistance = current.Distance(neighbor);
                    // The distance from start to a neighbor
                    var tentativeGScore = currentGScore + linkDistance;
                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    } else if (tentativeGScore >= neightborGScore) {
                        // This is not a better path.
                        continue;
                    }
                    // This path is the best until now. Record it!
                    cameFrom[neighbor] = link;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + neighbor.Distance(to);
                }
            }
            //Failure
            Debug.LogWarning("Couldn't find a path from " + from + " to " + to);
            return Reconstruct(cameFrom, current);
        }

        private static List<ILink> Reconstruct(IDictionary<Node, ILink> cameFrom, Node current) {
            var currentCameFrom = cameFrom[current];
            var totalPath = new List<ILink> {
                currentCameFrom
            };
            while (cameFrom.ContainsKey(current)) {
                var link = cameFrom[current];
                current = link.From;
                totalPath.Add(link);
            }
            return totalPath;
        }
    }
}