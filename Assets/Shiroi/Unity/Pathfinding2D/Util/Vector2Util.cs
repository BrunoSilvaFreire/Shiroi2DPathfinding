using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public static class Vector2Util {
        public static bool IsWithin(this Vector2 position, MapPosition max, MapPosition min) {
            return position.x <= max.X && position.x >= min.X && position.y >= min.Y && position.y <= max.Y;
        }

        public static bool IsWithin(this Vector2 position, Vector2 max, Vector2 min) {
            return position.x <= max.x && position.x >= min.x && position.y >= min.y && position.y <= max.y;
        }
    }
}