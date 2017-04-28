using System.Collections.Generic;
using Shiroi.Unity.Pathfinding2D.Link;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public class AStarPreviewer : BaseBehaviour {
        public Color FromColor = Color.green;
        public Color ToColor = Color.red;
        public Color PathColor = ColorUtil.FromHex("FFD800FF");
        public MapPosition From;
        public MapPosition To;
        public TileMap TileMap;
        public LinkMap LinkMap;
        public List<ILink> LastPath;

        public Node FromNode {
            get { return TileMap.GetNode(From); }
        }

        public Node ToNode {
            get { return TileMap.GetNode(To); }
        }

        [Show]
        public void SwapPositions() {
            var temp = From;
            From = To;
            To = temp;
        }

        [Show]
        public void RecalculatePath() {
            if (!TileMap) {
                Debug.LogWarning("You must provide a TileMap!");
                return;
            }
            if (!LinkMap) {
                Debug.LogWarning("You must provide a LinkMap!");
                return;
            }
            var from = FromNode;
            var to = ToNode;

            if (from == null) {
                Debug.LogWarning("Couldn't find a node (From) @ " + From);
                return;
            }
            if (to == null) {
                Debug.LogWarning("Couldn't find a node (To) @ " + To);
                return;
            }
            LastPath = AStarPathfinding.CalculatePath(from, to, LinkMap);
            if (LastPath == null) {
                Debug.LogWarning("Couldn't find a path!");
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = FromColor;
            From.DrawGizmos();
            Gizmos.color = ToColor;
            To.DrawGizmos();
            if (LastPath != null) {
                Gizmos.color = PathColor;
                for (var i = 1; i < LastPath.Count; i++) {
                    var point = LastPath[i];
                    var previous = LastPath[i - 1];
                    previous.DrawGizmos();
                }
            }
        }
    }
}