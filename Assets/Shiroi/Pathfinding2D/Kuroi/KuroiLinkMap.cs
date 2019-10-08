using System;
using System.Collections;
using System.Collections.Generic;
using Shiroi.Pathfinding2D.Runtime;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Kuroi {
    public class KuroiLinkMap : LinkMap2D<KuroiLinkMap.LinkNode, KuroiNavMesh.GeometryNode> {
        public uint jumpCount = 3;

        public float timeStep = 0.01F;
        public Vector2 hitBoxSize = Vector2.one;
        public Vector2 maxForce;

        [Serializable]
        public struct LinkNode {
            public static readonly LinkNode Empty = new LinkNode {
                gravitationalLinks = new GravitationalLink[0],
                directLinks = new DirectLink[0]
            };

            [Serializable]
            public class GravitationalLink : ILink {
                [SerializeField]
                private Vector2 force;

                [SerializeField]
                private Vector2[] path;

                [SerializeField]
                private uint destination;

                public override string ToString() {
                    return $"Gravitational({nameof(destination)}: {destination})";
                }

                public GravitationalLink(Vector2 force, Vector2[] path, uint destination) {
                    this.force = force;
                    this.path = path;
                    this.destination = destination;
                }

                public uint Destination => destination;
                public float CalculateCost<L, G>(LinkMap2D<L, G> linkMap2D) {
                    var sum = 0.0F;
                    for (int i = 1; i < path.Length; i++) {
                        sum += Vector2.Distance(path[i - 1], path[i]);
                    }
                    return sum;
                }

                public Vector2 Force => force;

                public Vector2[] Path => path;
            }

            public DirectLink[] directLinks;
            public GravitationalLink[] gravitationalLinks;

            public IEnumerable<ILink> Links {
                get {
                    foreach (var directLink in directLinks) {
                        yield return directLink;
                    }

                    foreach (var link in gravitationalLinks) {
                        yield return link;
                    }
                }
            }
        }

        public override LinkNode Generate(int x, int y) {
            var origin = NavMesh.NodeUnsafe(x, y);
            if (origin.IsSolid()) {
                return LinkNode.Empty;
            }

            var linearLinks = new List<DirectLink>();
            var gravitationalLinks = new List<LinkNode.GravitationalLink>();
            TryConnect(x, y + 1, linearLinks);
            TryConnect(x, y - 1, linearLinks);
            TryConnect(x - 1, y, linearLinks);
            TryConnect(x + 1, y, linearLinks);
            if (origin.IsSupported()) {
                PopulateJumpLinks(x, y, gravitationalLinks);
            }

            return new LinkNode {
                directLinks = linearLinks.ToArray(),
                gravitationalLinks = gravitationalLinks.ToArray()
            };
        }

        private bool ApproximateJump(
            uint index,
            int height,
            Vector2 origin,
            Vector2 force,
            out LinkNode.GravitationalLink link
        ) {
            var currentPosition = origin;
            RaycastHit2D cast;
            var points = new List<Vector2> {
                currentPosition
            };
            do {
                var hitboxCenter = currentPosition;
                hitboxCenter.y += hitBoxSize.y / 2 + 0.1F;
                cast = Physics2D.BoxCast(
                    hitboxCenter,
                    hitBoxSize,
                    0,
                    force,
                    force.magnitude * timeStep,
                    NavMesh.worldMask
                );
                currentPosition += force * timeStep;
                force += Physics2D.gravity * timeStep;
                points.Add(cast.collider == null ? currentPosition : cast.point);

                if (NavMesh.IsOutOfBounds(currentPosition)) {
                    link = default;
                    return false;
                }
            } while (cast.collider == null);

            if (cast.normal.y <= 0) {
                link = default;
                return false;
            }

            var cellPos = NavMesh.grid.WorldToCell(cast.point);
            if (!NavMesh.IsOutOfBounds(cellPos.x, cellPos.y)) {
                var dest = NavMesh.IndexOfUnsafe(cellPos.x, cellPos.y);
                if (cellPos.y != height && index != dest) {
                    link = new LinkNode.GravitationalLink(force, points.ToArray(), dest);
                    return true;
                }
            }

            link = default;
            return false;
        }

        private void CastJumpDirection(
            uint index,
            int height,
            Vector2 center,
            int xDir,
            List<LinkNode.GravitationalLink> output
        ) {
            var visited = new List<uint>();
            for (var x = 0; x < jumpCount; x++) {
                for (var y = 0; y <= jumpCount; y++) {
                    var fx = (float) (x + 1) / jumpCount * xDir;
                    var fy = (float) y / jumpCount;
                    var force = new Vector2(fx * maxForce.x, fy * maxForce.y);
                    if (!ApproximateJump(index, height, center, force, out var link)) {
                        continue;
                    }

                    if (visited.Contains(link.Destination)) {
                        continue;
                    }

                    output.Add(link);
                    visited.Add(link.Destination);
                }
            }
        }

        private void PopulateJumpLinks(int x, int y, List<LinkNode.GravitationalLink> output) {
            var i = NavMesh.IndexOf(x, y);
            var offset = new Vector2(NavMesh.grid.cellSize.x / 2, 0);
            var center = (Vector2) NavMesh.grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            var right = center + offset;
            var left = center - offset;
            CastJumpDirection(i, y, right, 1, output);
            CastJumpDirection(i, y, left, -1, output);
        }

        private void TryConnect(int x, int y, List<DirectLink> output) {
            if (NavMesh.IsOutOfBounds(x, y)) {
                return;
            }

            var neighbor = NavMesh.NodeUnsafe(x, y);
            if (!neighbor.IsSolid()) {
                output.Add(new DirectLink(NavMesh.IndexOfUnsafe(x, y)));
            }
        }
    }
}