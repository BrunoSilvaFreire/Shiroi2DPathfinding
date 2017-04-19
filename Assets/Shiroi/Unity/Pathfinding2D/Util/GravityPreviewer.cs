using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Util {
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityPreviewer : BaseBehaviour {
        public Color Color = Color.green;
        private Vector2 speedModifier = Vector2.one;
        public TileMap TileMap;
        public float TimeIncrementation = 0.1f;
        public Node EndNode;
        public IGroundEntity Entity;

        [Show]
        public Vector2 SpeedModifier {
            get { return speedModifier; }
            set { speedModifier = value.normalized; }
        }

        private void OnDrawGizmosSelected() {
            if (!TileMap || Entity == null || !Entity.RigidBody) {
                return;
            }
            var speed = new Vector2(Entity.MaxSpeed * SpeedModifier.x, Entity.JumpHeight * SpeedModifier.y);
            var path = GravityUtil.CalculatePath(transform.position, speed, TimeIncrementation, TileMap, Entity,
                out EndNode);
            Gizmos.color = Color;
            GravityUtil.DrawGizmos(path);
        }
    }
}