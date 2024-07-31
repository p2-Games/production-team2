///<summary>
/// Author: Halen
///
/// Moves an object through an array of transforms, the direction determined on the active state.
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace MovingLevelObjects
        {
            public class TriggeredPlatform : MovingLevelObject
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

                [Header("Platform Details"), Tooltip("If the platform should start at the end of its path rather than the beginning.")]
                [SerializeField] private bool m_startAtEnd = false;
                [Tooltip("The platform moves from the start of this array to the end.")]
                [SerializeField] private Transform[] m_path;

                private Vector3 m_direction;
                private int m_targetIndex;

                // returns if this object is at the provided position
                private bool IsAtTransform(int index)
                {
                    return Vector3.Distance(transform.position, m_path[index].position) < m_movementSpeed * Time.fixedDeltaTime;
                }

                private Vector3 GetDirectionToTarget()
                {
                    return (m_path[m_targetIndex].position - transform.position).normalized;
                }

                protected override void Start()
                {
                    base.Start();
                    GetDirectionToTarget();
                    if (m_startAtEnd)
                    {
                        m_targetIndex = m_path.Length - 1;
                        transform.position = m_path[m_targetIndex].position;
                    }
                    else
                    {
                        m_targetIndex = 0;
                        transform.position = m_path[0].position;
                    }
                }

                private void FixedUpdate()
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

#if UNITY_EDITOR
                [ContextMenu("Move Platform to Start")]
                private void SetPlatformToInitialPosition()
                {
                    if (m_path.Length > 1)
                        transform.position = m_path[0].position;
                    else
                        Debug.LogError("A platform's path require's at least two Transforms.");
                }

                [ContextMenu("Move Platform to End")]
                private void SetPlatformToTerminalPosition()
                {
                    if (m_path.Length > 1)
                        transform.position = m_path[m_path.Length - 1].position;
                    else
                        Debug.LogError("A platform's path require's at least two Transforms.");
                }

                private void OnDrawGizmos()
                {
                    for (int t = 0; t < m_path.Length; t++)
                    {
                        if (t == m_targetIndex)
                            Handles.color = Color.green;
                        else
                            Handles.color = Color.white;

                        Gizmos.DrawWireCube(m_path[t].position, Vector3.one);
                    }
                }
#endif
            }
        }
    }
}
