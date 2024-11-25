///<summary>
/// Author: Halen
///
/// Spins when it is active.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace MovingObjects
        {
            public class SpinningObject : MovingObject
            {
                protected override void FixedUpdate()
                {
                    if (m_isActive)
                    {
                        Quaternion newRotation = transform.rotation * Quaternion.AngleAxis(m_movementSpeed, transform.up);
                        m_rb.MoveRotation(newRotation);
                    }
                }
            }
        }
    }
}
