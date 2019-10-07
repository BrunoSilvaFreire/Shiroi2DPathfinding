using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Runtime {
    public class LinkMap2D : MonoBehaviour {
        [Serializable]
        public struct LinkNode {
            public static readonly LinkNode Empty = new LinkNode {
                gravitationalLinks = new GravitationalLink[0],
                directLinks = new DirectLink[0]
            };

            [Serializable]
            public struct DirectLink : ILink {
                [SerializeField]
                private uint destination;

                public DirectLink(uint destination) {
                    this.destination = destination;
                }

                public uint Destination => destination;
            }

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


        public NavMesh2D navMesh;
        public LinkNode[] links;
    }
}