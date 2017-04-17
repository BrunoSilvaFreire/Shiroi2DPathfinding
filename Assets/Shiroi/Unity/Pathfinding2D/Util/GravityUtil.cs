using System.Collections.Generic;
using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public static class GravityUtil {
        public const float DefaultGravityForce = 9.8f;

        public static List<Vector2> CalculatePath(
            Vector2 pos,
            Vector2 initialSpeed,
            float timeIncrementation,
            TileMap tileMap,
            IGroundEntity entity,
            out Node finalNode,
            float gravityForce = DefaultGravityForce) {
            var minY = tileMap.MinY;
            finalNode = null;
            var rb = entity.RigidBody;
            var gravity = rb.mass * -gravityForce * rb.gravityScale;
            var list = new List<Vector2>();
            var time = 0F;
            while (finalNode == null) {
                var y = pos.y + initialSpeed.y * time + gravity * Mathf.Pow(time, 2) / 2;
                if (y < minY) {
                    break;
                }
                var x = pos.x + initialSpeed.x * time;
                list.Add(new Vector2(x, y));
                time += timeIncrementation;
            }
            return list;
        }

        public static void DrawGizmos(List<Vector2> path) {
            for (var i = 1; i < path.Count - 1; i++) {
                var point = path[i];
                var previous = path[i - 1];
                Gizmos.DrawLine(previous, point);
            }
        }
    }
}