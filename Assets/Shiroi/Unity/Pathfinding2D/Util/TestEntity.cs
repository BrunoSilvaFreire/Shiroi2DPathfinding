using UnityEngine;
using Vexe.Runtime.Types;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public class TestEntity : BaseBehaviour, IGroundEntity {
        public TestEntity() {
            MaxSpeed = 2f;
            JumpHeight = 5F;
        }

        [Show]
        public Rigidbody2D RigidBody {
            get;
            private set;
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
    }
}