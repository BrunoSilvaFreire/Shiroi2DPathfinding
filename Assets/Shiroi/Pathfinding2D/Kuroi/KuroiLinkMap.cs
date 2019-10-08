using System;
using System.Collections.Generic;
using Shiroi.Pathfinding2D.Runtime;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Kuroi {
    public class KuroiLinkMap : LinkMap2D<KuroiLinkMap.LinkNode, KuroiNavMesh.GeometryNode> {
        public uint jumpCount = 3;

        public float timeStep = 0.01F;
        public Vector2 hitBoxSize = Vector2.one;

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

                public GravitationalLink(Vector2 force, Vector2[] path, uint destination) {
                    this.force = force;
                    this.path = path;
                    this.destination = destination;
                }

                public uint Destination => destination;

                public Vector2 Force => force;

                public Vector2[] Path => path;
            }

            public DirectLink[] directLinks;
            public GravitationalLink[] gravitationalLinks;
        }

        public override LinkNode Generate(int x, int y) {
            var origin = navMesh.NodeUnsafe(x, y);
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
                GenerateJumpLink(x, y, gravitationalLinks);
            }

            return new LinkNode {
                directLinks = linearLinks.ToArray(),
                gravitationalLinks = gravitationalLinks.ToArray()
            };
        }

        private bool CastJumpLink(
            uint index,
            int height,
            Vector2 origin,
            Vector2 force,
            Vector2 hitbox,
            List<uint> blacklist,
            out LinkNode.GravitationalLink link
        ) {
            var currentPosition = origin;
            RaycastHit2D cast;
            var points = new List<Vector2> {
                currentPosition
            };
            do {
                cast = Physics2D.BoxCast(
                    currentPosition,
                    hitbox,
                    0,
                    force,
                    force.magnitude * timeStep,
                    navMesh.worldMask
                );
                cast = Physics2D.Linecast(
                    currentPosition,
                    currentPosition + force,
                    navMesh.worldMask
                );
                currentPosition += force * timeStep;
                force += Physics2D.gravity * timeStep;
                points.Add(cast.collider == null ? currentPosition : cast.point);

                if (navMesh.IsOutOfBounds(currentPosition)) {
                    link = default;
                    return false;
                }
            } while (cast.collider == null);

            if (cast.normal.y <= 0) {
                link = default;
                return false;
            }

            var cellPos = navMesh.grid.WorldToCell(cast.point);

            var dest = navMesh.IndexOf(cellPos.x, cellPos.y);
            if (cellPos.y == height || index == dest || blacklist.Contains(dest)) {
                link = default;
                return false;
            }


            link = new LinkNode.GravitationalLink(force, points.ToArray(), dest);
            return true;
        }

        private void CastJumpDirection(
            uint index,
            int height,
            Vector2 center,
            int xDir,
            Vector2 hitbox,
            List<LinkNode.GravitationalLink> links
        ) {
            var blacklist = new List<uint>();
            for (var x = 0; x < jumpCount; x++) {
                for (var y = 0; y <= jumpCount; y++) {
                    var fx = (float) (x + 1) / jumpCount * xDir;
                    var fy = (float) y / jumpCount;
                    var force = new Vector2(fx * 8, fy * 12);
                    if (!CastJumpLink(index, height, center, force, hitbox, blacklist, out var link)) {
                        continue;
                    }

                    links.Add(link);
                    blacklist.Add(link.Destination);
                }
            }
        }

        private void GenerateJumpLink(int x, int y,
            List<LinkNode.GravitationalLink> links) {
            var i = navMesh.IndexOf(x, y);
            var offset = new Vector2(navMesh.grid.cellSize.x / 2, 0);
            var center = (Vector2) navMesh.grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            var right = center + offset;
            var left = center - offset;
            CastJumpDirection(i, y, right, 1, hitBoxSize, links);
            CastJumpDirection(i, y, left, -1, hitBoxSize, links);
        }

        private void TryConnect(int x, int y, List<DirectLink> links) {
            if (navMesh.IsOutOfBounds(x, y)) {
                return;
            }

            var neighbor = navMesh.NodeUnsafe(x, y);
            if (!neighbor.IsSolid()) {
                links.Add(new DirectLink(navMesh.IndexOfUnsafe(x, y)));
            }
        }
    }
}