using System;
using System.Collections.Generic;
using Shiroi.Unity.Pathfinding2D.Exception;
using Shiroi.Unity.Pathfinding2D.Util;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D {
    [Serializable]
    public class Platform {
        [Show]
        private readonly List<Node> nodes = new List<Node>();

        public Platform(Node position) {
            LeftEdgeNode = position;
            RightEdgeNode = position;
            Color = ColorUtil.Random(155);
        }

        public void DrawGizmos() {
            Gizmos.color = Color;

            Gizmos.DrawWireCube(Center, Size);
            if (XSize > 1) {
                Gizmos.DrawCube(Center, Vector2.one);
            }
        }

        public Vector2 Size {
            get { return new Vector2(XSize, 1); }
        }

        public Vector2 Center {
            get { return new Vector2(XCenter, Y); }
        }

        [Show]
        public float XCenter {
            get { return (float) (LeftX + RightX) / 2; }
        }

        [Show]
        public Color Color {
            get;
            set;
        }

        [Show]
        public Node LeftEdgeNode {
            get;
            private set;
        }

        [Show]
        public Node RightEdgeNode {
            get;
            private set;
        }

        private bool IsDefined {
            get { return LeftEdgeNode != null && RightEdgeNode != null; }
        }

        [Show]
        public int Y {
            get {
                if (!IsDefined) {
                    throw new PlatformNotDefinedException();
                }
                var y = LeftEdgeNode.Position.Y;
                if (y != RightEdgeNode.Position.Y) {
                    throw new InvalidOperationException(
                        "The LeftEdgeNode and RightEdgeNode are in different heights! (Y)");
                }
                return y;
            }
        }

        [Show]
        public int LeftX {
            get {
                if (!IsDefined) {
                    throw new PlatformNotDefinedException();
                }
                return LeftEdgeNode.Position.X;
            }
        }

        [Show]
        public int RightX {
            get {
                if (!IsDefined) {
                    throw new PlatformNotDefinedException();
                }
                return RightEdgeNode.Position.X;
            }
        }

        public int XSize {
            get {
                if (!IsDefined) {
                    throw new PlatformNotDefinedException();
                }
                return RightX - LeftX + 1;
            }
        }

        public List<Node> Nodes {
            get { return nodes; }
        }

        public List<Node> AllNodes {
            get {
                var n = nodes;
                n.Add(LeftEdgeNode);
                n.Add(RightEdgeNode);
                return n;
            }
        }

        public void AddNode(Node node) {
            if (Contains(node)) {
                Debug.LogWarning("Tried to add node " + node + " to platform " + this + ", but is already contained!");
                return;
            }
            node.Platform = this;
            if (LeftEdgeNode == null || node.X < LeftX) {
                LeftEdgeNode = node;
            }
            if (RightEdgeNode == null || node.X > RightX) {
                RightEdgeNode = node;
            }
            if (node != RightEdgeNode && node != LeftEdgeNode) {
                Nodes.Add(node);
            }
        }

        public bool IsNextToAndAdd(Node node, int limit = 1) {
            var isNextTo = IsNextTo(node, limit);
            if (isNextTo && !Contains(node)) {
                AddNode(node);
            }
            return isNextTo;
        }

        public bool IsNextTo(Node node, int limit = 1) {
            return node.X >= LeftX - limit && node.X <= RightX + limit && node.Y == Y;
        }

        public bool Contains(Node node) {
            return node == LeftEdgeNode || node == RightEdgeNode || Nodes.Contains(node);
        }

        public override string ToString() {
            return string.Format("Platform(Y={0}, LeftX={1}, RightX={2})", Y, LeftX, RightX);
        }

        public void Merge(Platform platform) {
            foreach (var node in platform.AllNodes) {
                AddNode(node);
            }
        }
    }
}