using System;
using UnityEditor;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D {
    [Serializable]
    public class MapPosition : IComparable<MapPosition> {
        [Show]
        public int X {
            get;
            set;
        }

        [Show]
        public int Y {
            get;
            set;
        }

        public MapPosition(int x, int y) {
            X = x;
            Y = y;
        }

        public Vector2 ToWorldPosition(Vector2 vectorSize) {
            return new Vector2(vectorSize.x * X, vectorSize.y * Y);
        }

        public static implicit operator Vector2(MapPosition pos) {
            return new Vector2(pos.X, pos.Y);
        }

        public static implicit operator MapPosition(Vector2 pos) {
            return new MapPosition((int) pos.x, (int) pos.y);
        }

        public void SetPosition(Vector2 vector) {
            X = (int) vector.x;
            Y = (int) vector.y;
        }

        protected bool Equals(MapPosition other) {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((MapPosition) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (X * 397) ^ Y;
            }
        }

        public int CompareTo(MapPosition other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var xComparison = X.CompareTo(other.X);
            return xComparison != 0 ? xComparison : Y.CompareTo(other.Y);
        }

        public bool IsWithin(MapPosition max, MapPosition min) {
            return X >= max.X && X <= min.X && Y >= min.Y && Y <= max.Y;
        }

        public bool IsWithin(Vector2 max, Vector2 min) {
            return X >= max.x && X <= min.x && Y >= min.y && Y <= max.y;
        }
    }
}