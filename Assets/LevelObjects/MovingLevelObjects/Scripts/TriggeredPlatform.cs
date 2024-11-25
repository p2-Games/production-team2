///<summary>
/// Author: Halen
///
/// Moves an object to the end of its path when active and to the start of its path when inactive.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace MovingObjects
        {
            public class TriggeredPlatform : MovingObject
            {
                public override bool isActive
                {
                    get => base.isActive;
                    set
                    {
                        if (m_isActive != value)
                        {
                            if (value)
                                m_targetIndex++;
                            else
                                m_targetIndex--;

                            m_direction = GetDirectionToTarget();
                        }

                        m_isActive = value;
                    }
                }

                protected override void FixedUpdate()
                {
                    // check if platform has reached its target position
                    // if it is, move to the next index and calc next move direction
                    if (IsAtTransform(m_targetIndex))
                    {
                        // if moving towards end of array
                        if (m_isActive)
                        {
                            if (m_targetIndex != m_path.Length - 1)
                                m_targetIndex++;
                        }
                        // if moving towards start of array
                        else if (m_targetIndex != 0)
                            m_targetIndex--;

                        // direction does not change over time, so set it here
                        m_direction = GetDirectionToTarget();
                    }
                    // if not at target position, move the object towards its target position
                    else
                        m_rb.MovePosition(transform.position + m_direction * m_movementSpeed * Time.fixedDeltaTime);
                }
            }
        }
    }
}
