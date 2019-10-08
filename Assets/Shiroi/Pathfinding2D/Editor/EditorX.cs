using UnityEditor;
using UnityEngine;

namespace Shiroi.Pathfinding2D.Editor {
    public static class EditorX {
        public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            ForGizmo(pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle, arrowPosition);
        }

        public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);
            DrawArrowEnd(true, pos, direction, color, arrowHeadLength, arrowHeadAngle, arrowPosition);
        }

        public static void ForDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            ForDebug(pos, direction, Color.white, arrowHeadLength, arrowHeadAngle, arrowPosition);
        }

        public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            Debug.DrawRay(pos, direction, color);
            DrawArrowEnd(false, pos, direction, color, arrowHeadLength, arrowHeadAngle, arrowPosition);
        }

        private static void DrawArrowEnd(bool gizmos, Vector3 pos, Vector3 direction, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            var right =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back) *
                arrowHeadLength;
            var left =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back) *
                arrowHeadLength;
            var up = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back) *
                     arrowHeadLength;
            var down =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back) *
                arrowHeadLength;

            var arrowTip = pos + (direction * arrowPosition);

            if (gizmos) {
                Gizmos.color = color;
                Gizmos.DrawRay(arrowTip, right);
                Gizmos.DrawRay(arrowTip, left);
                Gizmos.DrawRay(arrowTip, up);
                Gizmos.DrawRay(arrowTip, down);
            }
            else {
                Debug.DrawRay(arrowTip, right, color);
                Debug.DrawRay(arrowTip, left, color);
                Debug.DrawRay(arrowTip, up, color);
                Debug.DrawRay(arrowTip, down, color);
            }
        }

        public static void ForHandles(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f, float arrowPosition = 1.0f) {
            Handles.color = color;
            Handles.DrawLine(pos, pos + direction);
            DrawArrowEndHandles(pos, direction, color, arrowHeadLength, arrowHeadAngle, arrowPosition);
        }

        private static void DrawArrowEndHandles(Vector3 pos, Vector3 direction, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f) {
            var right =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back) *
                arrowHeadLength;
            var left =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back) *
                arrowHeadLength;
            var up = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back) *
                     arrowHeadLength;
            var down =
                (Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back) *
                arrowHeadLength;

            var arrowTip = pos + direction * arrowPosition;


            Handles.DrawLine(arrowTip, right + arrowTip);
            Handles.DrawLine(arrowTip, left + arrowTip);
            Handles.DrawLine(arrowTip, up + arrowTip);
            Handles.DrawLine(arrowTip, down + arrowTip);
        }

        public static void DrawBoundaries(Vector2Int minCell, Vector2Int maxCell, Grid navmeshGrid) {
            var min = navmeshGrid.GetCellCenterWorld((Vector3Int) minCell);
            var max = navmeshGrid.GetCellCenterWorld((Vector3Int) maxCell);
            Handles.DrawDottedLines(
                new[] {
                    new Vector3(min.x, min.y),
                    new Vector3(min.x, max.y),
                    new Vector3(max.x, max.y),
                    new Vector3(max.x, min.y)
                },
                new[] {
                    0, 1,
                    1, 2,
                    2, 3,
                    3, 0
                },
                UnityX.kBoundariesWidth
            );
        }
    }
}