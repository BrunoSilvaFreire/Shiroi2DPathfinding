using System;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Runtime {
    public interface ILink {
        uint Destination { get; }
    }

    [Serializable]
    public struct DirectLink : ILink {
        [SerializeField]
        private uint destination;

        public DirectLink(uint destination) {
            this.destination = destination;
        }

        public uint Destination => destination;
    }
}