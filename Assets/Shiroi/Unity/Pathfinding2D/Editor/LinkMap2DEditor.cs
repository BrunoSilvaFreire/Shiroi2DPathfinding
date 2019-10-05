using System.Collections.Generic;
using Shiroi.Unity.Pathfinding2D.Runtime;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Editor {
    [CustomEditor(typeof(LinkMap2D))]
    public partial class LinkMap2DEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var linkMap = (LinkMap2D) target;
            linkMap.navMesh =
                (NavMesh2D) EditorGUILayout.ObjectField("Navmesh", linkMap.navMesh, typeof(NavMesh2D), true);
            GUI.enabled = linkMap.navMesh != null;
            if (GUILayout.Button("Generate Links")) {
                GenerateLinks(linkMap);
            }
        }

        public void GenerateLinks(LinkMap2D linkMap) {
            var navmesh = linkMap.navMesh;
            var min = navmesh.Min;
            var max = navmesh.Max;
            var nodes = new LinkMap2D.LinkNode[navmesh.Area];
            var linearLinks = new List<LinkMap2D.LinkNode.DirectLink>();
            for (var x = min.x; x <= max.x; x++) {
                for (var y = min.y; y <= max.y; y++) {
                    linearLinks.Clear();
                    var origin = navmesh.NodeUnsafe(x, y);
                    if (origin.IsSolid()) {
                        continue;
                    }

                    var index = navmesh.IndexOfUnsafe(x, y);
                    TryConnect(x, y + 1, navmesh, linearLinks);
                    TryConnect(x, y - 1, navmesh, linearLinks);
                    TryConnect(x - 1, y, navmesh, linearLinks);
                    TryConnect(x + 1, y, navmesh, linearLinks);
                    nodes[index].directLinks = linearLinks.ToArray();
                }
            }

            linkMap.links = nodes;
        }

        private static void TryConnect(int x, int y, NavMesh2D navmesh, List<LinkMap2D.LinkNode.DirectLink> links) {
            if (navmesh.IsOutOfBounds(x, y)) {
                return;
            }

            var neightboor = navmesh.NodeUnsafe(x, y);
            if (!neightboor.IsSolid()) {
                links.Add(new LinkMap2D.LinkNode.DirectLink {
                    destination = navmesh.IndexOfUnsafe(x, y)
                });
            }
        }
    }
}