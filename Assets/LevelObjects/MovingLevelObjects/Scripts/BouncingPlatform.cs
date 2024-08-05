///<summary>
/// Author: Halen
///
/// A moving object that bounces back and forth along its path.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace MovingLevelObjects
        {
            public class BouncingPlatform : MovingLevelObject
            {
                [Header("Bouncing Platform Details"), Tooltip("If the platform is not at the start or end of it's path, you can choose which way it will move on Start.")]
                [SerializeField] private bool m_startsMovingBackward;
                private bool m_movingToStart;

                protected override void Start()
                {
                    base.Start();

                    if (IsAtTransform(0))
                        m_movingToStart = false;
                    else if (IsAtTransform(m_path.Length - 1))
                        m_movingToStart = true;
                    else
                        m_movingToStart = m_startsMovingBackward;
                }

                protected override void FixedUpdate()
                {
                    // if the platform is inactive, don't move it
                    if (!m_isActive)
                        return;

                    // check if platform has reached its target position
                    // if it is, move to the next index and calc next move direction
                    if (IsAtTransform(m_targetIndex))
                    {
                        // if at the start or the end of the path, change direction
                        if (m_targetIndex == 0 || m_targetIndex == m_path.Length - 1)
                            m_movingToStart = !m_movingToStart;

                        // increment target index
                        if (m_movingToStart)
                            m_targetIndex++;
                        else
                            m_targetIndex--;

                        // calc direction to move
                        m_direction = GetDirectionToTarget();
                    }
                    else
                        m_rb.MovePosition(transform.position + m_direction * m_movementSpeed * Time.fixedDeltaTime);
                }
            }
        }
    }
}
