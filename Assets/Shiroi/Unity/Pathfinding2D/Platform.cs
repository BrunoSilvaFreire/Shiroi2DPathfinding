using System;
using System.Collections.Generic;
using Shiroi.Unity.Pathfinding2D.Exception;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D {
    public class Platform {
        private List<Node> nodes = new List<Node>();

        public Node LeftEdgeNode {
            get;
            private set;
        }

        public Node RightEdgeNode {
            get;
            private set;
        }

        private bool IsDefined {
            get {
                return LeftEdgeNode != null && RightEdgeNode != null;
            }
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
                return LeftEdgeNode.Position.X;
            }
        }

        public int XSize {
            get {
                if (!IsDefined) {
                    throw new PlatformNotDefinedException();
                }
                return RightX - LeftX;
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
                nodes.Add(node);
            }
        }

        private bool Contains(Node node) {
            return node == LeftEdgeNode || node == RightEdgeNode || nodes.Contains(node);
        }

        public override string ToString() {
            return string.Format("Platform(Y={0}, LeftX={1}, RightX={2})", Y, LeftX, RightX);
        }
    }
}