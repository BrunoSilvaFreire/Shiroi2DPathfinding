using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D {
    public interface IGroundEntity {
        Rigidbody2D RigidBody {
            get;
        }

        float MaxSpeed {
            get;
        }

        float JumpHeight {
            get;
        }
    }
}