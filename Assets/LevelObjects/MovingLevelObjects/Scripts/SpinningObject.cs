///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace MovingLevelObjects
        {
            public class SpinningObject : MovingLevelObject
            {
                private void Update()
                {
                    m_rb.velocity = Vector3.zero;
                    m_rb.angularVelocity = transform.forward * m_movementSpeed;
                }

                protected override void FixedUpdate() { }
            }
        }
    }
}
