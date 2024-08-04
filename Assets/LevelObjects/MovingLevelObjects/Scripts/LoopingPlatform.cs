///<summary>
/// Author: Halen
///
/// A moving object that loops through its path.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace MovingLevelObjects
        {
            public class LoopingPlatform : MovingLevelObject
            {
                [Header("Bouncing Platform Details"), Tooltip("If the platform is not at the start or end of it's path, you can choose which way it will move on Start.")]
                [SerializeField] private bool m_startsMovingBackward;
                private bool m_movingToStart;

                protected override void Start()
                {
                    base.Start();

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
                        // increment target index
                        if (m_movingToStart)
                            m_targetIndex++;
                        else
                            m_targetIndex--;

                        // loop target index around when reaching either end of the path
                        if (m_targetIndex == m_path.Length)
                            m_targetIndex = 0;
                        if (m_targetIndex == -1)
                            m_targetIndex = m_path.Length - 1;

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
