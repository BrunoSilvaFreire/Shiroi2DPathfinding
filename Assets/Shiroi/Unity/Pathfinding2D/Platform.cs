using System;
using System.Collections.Generic;
using Shiroi.Unity.Pathfinding2D.Exception;
using Shiroi.Unity.Pathfinding2D.Util;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D {
    [Serializable]
    public class Platform {
        [SerializeField, Hide]
        private List<Node> nodes;

        [SerializeField, Hide]
        private Node leftEdgeNode;

        [SerializeField, Hide]
        private Node rightEdgeNode;

        [SerializeField]
        private Color color;

        public Platform() {
        }

        public Platform(Node position) {
            LeftEdgeNode = position;
            this.nodes = new List<Node>();
            RightEdgeNode = position;
            Color = ColorUtil.Random(155);
        }

        protected bool Equals(Platform other) {
            return Equals(leftEdgeNode, other.leftEdgeNode) && Equals(rightEdgeNode, other.rightEdgeNode);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Platform) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((leftEdgeNode != null ? leftEdgeNode.GetHashCode() : 0) * 397) ^
                       (rightEdgeNode != null ? rightEdgeNode.GetHashCode() : 0);
            }
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

        public float XCenter {
            get { return (float) (LeftX + RightX) / 2; }
        }

        public Color Color {
            get { return color; }
            set { color = value; }
        }

        public Node LeftEdgeNode {
            get { return leftEdgeNode; }
            private set { leftEdgeNode = value; }
        }

        public Node RightEdgeNode {
            get { return rightEdgeNode; }
            private set { rightEdgeNode = value; }
        }

        private bool IsDefined {
            get { return LeftEdgeNode != null && RightEdgeNode != null; }
        }

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

        public int LeftX {
            get {
                if (!IsDefined) {
                    throw new PlatformNotDefinedException();
                }
                return LeftEdgeNode.Position.X;
            }
        }

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
            if (LeftEdgeNode == null || node.X < LeftX) {
                var oldLeft = LeftEdgeNode;
                LeftEdgeNode = node;
                if (oldLeft != null) {
                    AddNode(oldLeft);
                }
            }
            if (RightEdgeNode == null || node.X > RightX) {
                var oldRight = RightEdgeNode;
                RightEdgeNode = node;
                if (oldRight != null) {
                    AddNode(oldRight);
                }
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
            if (node == null) {
                throw new ArgumentNullException("node");
            }
            return node.X >= LeftX - limit && node.X <= RightX + limit && node.Y == Y;
        }

        public bool Contains(Node node) {
            return node == LeftEdgeNode || node == RightEdgeNode || Nodes.Contains(node);
        }

        public void Merge(Platform platform) {
            foreach (var node in platform.AllNodes) {
                AddNode(node);
            }
        }
    }
}