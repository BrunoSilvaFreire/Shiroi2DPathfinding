using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vexe.Runtime.Extensions;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Util {
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityPreviewer : BaseBehaviour {
        public Color Color = ColorUtil.FromHex("00FF00", 155);
        private Vector2 speedModifier = Vector2.one;
        public TileMap TileMap;
        private float timeIncrementation = 0.1f;
        public Node EndNode;
        public IGroundEntity Entity;
        public bool ForceSpeedMagnitude1 = true;

        [Show]
        public Vector2 SpeedModifier {
            get { return speedModifier; }
            set { speedModifier = ForceSpeedMagnitude1 ? value.normalized : value; }
        }

        [Show]
        public float TimeIncrementation {
            get { return timeIncrementation; }
            set {
                if (value <= 0) {
                    return;
                }
                timeIncrementation = value;
            }
        }

        private void OnDrawGizmosSelected() {
            if (!TileMap || Entity == null || !Entity.RigidBody) {
                return;
            }
            var speed = new Vector2(Entity.MaxSpeed * SpeedModifier.x, Entity.JumpHeight * SpeedModifier.y);
            var path = CalculatePath(transform.position, speed, TimeIncrementation, TileMap, Entity, out EndNode);
            Gizmos.color = Color;
            GravityUtil.DrawGizmos(path);
            if (EndNode != null) {
                Gizmos.DrawCube(EndNode.Position, Vector2.one);
            }
        }

        public List<Vector2> CalculatePath(Vector2 pos, Vector2 initialSpeed, float timeIncrementation, TileMap tileMap,
            IGroundEntity entity, out Node finalNode, float gravityForce = GravityUtil.DefaultGravityForce) {
            finalNode = null;
            var rb = entity.RigidBody;
            var gravity = rb.mass * -gravityForce * rb.gravityScale;
            var list = new List<Vector2>();
            var time = 0F;
            while (finalNode == null) {
                var y = pos.y + initialSpeed.y * time + gravity * Mathf.Pow(time, 2) / 2;
                var x = pos.x + initialSpeed.x * time;
                if (tileMap.IsOutOfBounds(x, y)) {
                    break;
                }
                var newPos = new Vector2(x, y);
                if (!list.IsEmpty()) {
                    var last = list.Last();
                    var dir = newPos - last;
                    var raycast = Physics2D.Raycast(last, dir, dir.magnitude, tileMap.WorldMask);
                    if (raycast) {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(last, 0.2f);
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(newPos, 0.2f);
                        Gizmos.DrawRay(last, dir);
                        var hitPoint = raycast.point;
                        list.Add(hitPoint);
                        var hitNode = tileMap.GetNode(hitPoint);
                        if (hitNode != null) {
                            Gizmos.DrawWireSphere(hitPoint, 0.2f);
                            var nodePos = hitNode.Position;
                            Gizmos.color = Color.cyan;
                            var hitDirVec = hitPoint - nodePos;
                            Gizmos.DrawRay(nodePos, hitDirVec);
                            Gizmos.DrawWireCube(nodePos, Vector2.one);
                            var direc = Direction.FromVector(hitDirVec, 0.4f, 0.4f);
                            finalNode = tileMap.GetNode(hitNode, direc);
                            if (finalNode != null) {
                                list.Add(finalNode.Position);
                            }
                        }
                        break;
                    }
                }
                list.Add(newPos);
                time += timeIncrementation;
            }
            Gizmos.color = Color;
            return list;
        }
    }
}