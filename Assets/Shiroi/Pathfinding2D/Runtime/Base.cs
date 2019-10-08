using System;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Runtime {
    public interface ILink {
        uint Destination { get; }
        float CalculateCost<L, G>(LinkMap2D<L, G> linkMap2D);
    }

    [Serializable]
    public struct DirectLink : ILink {
        [SerializeField]
        private uint destination;

        public DirectLink(uint destination) {
            this.destination = destination;
        }

        public uint Destination => destination;
        public float CalculateCost<L, G>(LinkMap2D<L, G> linkMap2D) {
            return linkMap2D.NavMesh.grid.cellSize.x;
        }

        public override string ToString() {
            return $"Direct({nameof(destination)}: {destination})";
        }
    }
}