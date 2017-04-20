using System;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D {
    public struct Direction {
        public static readonly Direction Up = new Direction(DirectionValue.Zero, DirectionValue.Foward);
        public static readonly Direction Down = new Direction(DirectionValue.Zero, DirectionValue.Backward);
        public static readonly Direction Left = new Direction(DirectionValue.Backward, DirectionValue.Zero);
        public static readonly Direction Right = new Direction(DirectionValue.Foward, DirectionValue.Zero);

        public struct DirectionValue {
            public static readonly DirectionValue Foward = new DirectionValue(1);
            public static readonly DirectionValue Zero = new DirectionValue(0);
            public static readonly DirectionValue Backward = new DirectionValue(-1);

            private readonly int value;

            private DirectionValue(int value) {
                this.value = value;
            }

            public static implicit operator DirectionValue(int i) {
                switch (i) {
                    case 1:
                        return Foward;
                    case 0:
                        return Zero;
                    case -1:
                        return Backward;
                    default:
                        throw new ArgumentOutOfRangeException(i.ToString());
                }
            }

            public static implicit operator int(DirectionValue val) {
                return val.value;
            }

            public static DirectionValue FromVector(float val, float limit) {
                if (val > limit) {
                    return Foward;
                }
                return val < limit ? Backward : Zero;
            }
        }

        public DirectionValue X;
        public DirectionValue Y;

        public static Direction FromVector(Vector2 vector2, float xLimit, float yLimit) {
            var x = vector2.x;
            var y = vector2.y;
            return new Direction(DirectionValue.FromVector(x, xLimit),DirectionValue.FromVector(y, yLimit));
        }

        public Direction(DirectionValue x, DirectionValue y) {
            X = x;
            Y = y;
        }


        public static implicit operator Vector2(Direction dir) {
            return new Vector2(dir.X, dir.Y);
        }
    }
}