using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Util {
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityPreviewer : BaseBehaviour, IGroundEntity {
        [Show]
        public Rigidbody2D RigidBody {
            get;
            private set;
        }

        public GravityPreviewer() {
            MaxSpeed = 2f;
            JumpHeight = 5F;
        }

        public Color Color = Color.green;
        private Vector2 speedModifier = Vector2.one;
        public TileMap TileMap;
        public float TimeIncrementation = 0.1f;
        public Node EndNode;

        [Show]
        public Vector2 SpeedModifier {
            get {
                return speedModifier;
            }
            set {
                speedModifier = value.normalized;
            }
        }

        [Show]
        public float MaxSpeed {
            get;
            private set;
        }

        [Show]
        public float JumpHeight {
            get;
            private set;
        }

        private void OnDrawGizmosSelected() {
            if (!TileMap || !RigidBody) {
                return;
            }
            var speed = new Vector2(MaxSpeed * SpeedModifier.x, JumpHeight * SpeedModifier.y);
            var path = GravityUtil.CalculatePath(
                transform.position,
                speed,
                TimeIncrementation,
                TileMap,
                this,
                out EndNode);
            Gizmos.color = Color;
            GravityUtil.DrawGizmos(path);
        }
    }
}