using System.Collections.Generic;
using Shiroi.Unity.Pathfinding2D.Editor.Validation;
using Shiroi.Unity.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Editor {
    [CustomEditor(typeof(LinkMap2D))]
    public partial class LinkMap2DEditor : UnityEditor.Editor {
        public uint JumpCount = 3;

        public float TimeStep = 0.01F;

        public override void OnInspectorGUI() {
            var linkMap = (LinkMap2D) target;
            using (new EditorGroupScope("Link Map")) {
                linkMap.navMesh =
                    (NavMesh2D) EditorGUILayout.ObjectField("Navmesh", linkMap.navMesh, typeof(NavMesh2D), true);
            }

            using (new EditorGroupScope("Info")) {
                var l = linkMap.links;
                if (l != null) {
                    EditorGUILayout.LabelField("Link count", l.Length.ToString());
                }
            }

            using (new EditorGroupScope("Editor")) {
                GUI.enabled = Validators.HasNavMesh.Check(linkMap);
                linkRadius = EditorGUILayout.IntField("Preview Radius", linkRadius);
                drawDirect = EditorGUILayout.Toggle("Draw Direct", drawDirect);
                drawGravitational = EditorGUILayout.Toggle("Draw Gravitational", drawGravitational);
                if (GUILayout.Button("Generate Links")) {
                    GenerateLinks(linkMap);
                }

                GUI.enabled = true;

                Validators.ValidateInEditor(
                    linkMap,
                    Validators.HasNavMesh,
                    Validators.LinksMatchGeometry
                );
            }
        }

        public void GenerateLinks(LinkMap2D linkMap) {
            var navmesh = linkMap.navMesh;
            var min = navmesh.Min;
            var max = navmesh.Max;
            var nodes = new LinkMap2D.LinkNode[navmesh.Area];
            var linearLinks = new List<LinkMap2D.LinkNode.DirectLink>();
            var gravitationalLinks = new List<LinkMap2D.LinkNode.GravitationalLink>();
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    linearLinks.Clear();
                    gravitationalLinks.Clear();
                    var origin = navmesh.NodeUnsafe(x, y);
                    var index = navmesh.IndexOfUnsafe(x, y);
                    if (origin.IsSolid()) {
                        nodes[index] = LinkMap2D.LinkNode.Empty;
                        continue;
                    }

                    TryConnect(x, y + 1, navmesh, linearLinks);
                    TryConnect(x, y - 1, navmesh, linearLinks);
                    TryConnect(x - 1, y, navmesh, linearLinks);
                    TryConnect(x + 1, y, navmesh, linearLinks);
                    if (origin.IsSupported()) {
                        GenerateJumpLink(navmesh, x, y, gravitationalLinks);
                    }

                    nodes[index] = new LinkMap2D.LinkNode {
                        directLinks = linearLinks.ToArray(),
                        gravitationalLinks = gravitationalLinks.ToArray()
                    };
                }
            }

            linkMap.links = nodes;
        }

        private bool CastJumpLink(
            NavMesh2D navmesh,
            uint index,
            int height,
            Vector2 origin,
            Vector2 force,
            Vector2 hitbox,
            List<uint> blacklist,
            out LinkMap2D.LinkNode.GravitationalLink link
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
                    force.magnitude * TimeStep,
                    navmesh.worldMask
                );
                /*cast = Physics2D.Linecast(
                    currentPosition,
                    currentPosition + force,
                    navmesh.worldMask
                );*/
                currentPosition += force * TimeStep;
                force += Physics2D.gravity * TimeStep;
                points.Add(cast.collider == null ? currentPosition : cast.point);

                if (navmesh.IsOutOfBounds(currentPosition)) {
                    link = default(LinkMap2D.LinkNode.GravitationalLink);
                    return false;
                }
            } while (cast.collider == null);

            if (cast.normal.y <= 0) {
                link = default(LinkMap2D.LinkNode.GravitationalLink);
                return false;
            }

            var cellPos = navmesh.grid.WorldToCell(cast.point);

            var dest = navmesh.IndexOf(cellPos.x, cellPos.y);
            if (cellPos.y == height || index == dest || blacklist.Contains(dest)) {
                link = default(LinkMap2D.LinkNode.GravitationalLink);
                return false;
            }


            link = new LinkMap2D.LinkNode.GravitationalLink(force, points.ToArray(), dest);
            return true;
        }

        private void CastJumpDirection(
            NavMesh2D navmesh,
            uint index,
            int height,
            Vector2 center,
            int xDir,
            Vector2 hitbox,
            List<LinkMap2D.LinkNode.GravitationalLink> links
        ) {
            var blacklist = new List<uint>();
            for (var x = 0; x < JumpCount; x++) {
                for (var y = 0; y <= JumpCount; y++) {
                    var fx = (float) (x + 1) / JumpCount * xDir;
                    var fy = (float) y / JumpCount;
                    var force = new Vector2(fx * 8, fy * 12);
                    if (CastJumpLink(navmesh, index, height, center, force, hitbox, blacklist, out var link)) {
                        links.Add(link);
                        blacklist.Add(link.Destination);
                    }
                }
            }
        }

        private void GenerateJumpLink(NavMesh2D navmesh, int x, int y,
            List<LinkMap2D.LinkNode.GravitationalLink> links) {
            var i = navmesh.IndexOf(x, y);
            var offset = new Vector2(navmesh.grid.cellSize.x / 2, 0);
            var center = (Vector2) navmesh.grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            var right = center + offset;
            var left = center - offset;
            var hitbox = navmesh.grid.cellSize * NavMesh2DEditor.kDefaultBoxcastSize;
            CastJumpDirection(navmesh, i, y, right, 1, hitbox, links);
            CastJumpDirection(navmesh, i, y, left, -1, hitbox, links);
        }

        private static void TryConnect(int x, int y, NavMesh2D navmesh, List<LinkMap2D.LinkNode.DirectLink> links) {
            if (navmesh.IsOutOfBounds(x, y)) {
                return;
            }

            var neighbor = navmesh.NodeUnsafe(x, y);
            if (!neighbor.IsSolid()) {
                links.Add(new LinkMap2D.LinkNode.DirectLink(navmesh.IndexOfUnsafe(x, y)));
            }
        }
    }
}