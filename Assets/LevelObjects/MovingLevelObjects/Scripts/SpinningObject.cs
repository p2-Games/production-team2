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
                protected override void FixedUpdate()
                {
                    Quaternion newRotation = transform.rotation * Quaternion.AngleAxis(m_movementSpeed, transform.up);
                    m_rb.MoveRotation(newRotation);
                }
            }
        }
    }
}
