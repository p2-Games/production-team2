///<summary>
/// Author: Halen
///
/// Moves an object through an array of transforms, the direction determined on the active state.
///
///</summary>

using Cinemachine;
using System.Collections;
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
                        }

                        m_isActive = value;
                    }
                }

                [Tooltip("How close the platform must be to its target position for it to count as being there.")]
                [SerializeField, Range(0, 0.2f)] private float m_distanceRequirement;

                [Tooltip("The platform moves from the start of this array to the end.")]
                [SerializeField] private Transform[] m_path;

                private int m_targetIndex;

                // returns the position of the transform from the path's index
                private Vector3 GetPosition(int index)
                {
                    return m_path[index].position;
                }

                private void Update()
                {
                    // returns if this object is at the provided position
                    bool IsAtPosition(Vector3 target)
                    {
                        return (target - transform.position).sqrMagnitude <= m_distanceRequirement * m_distanceRequirement;
                    }

                    // check if platform has reached its target position
                    // if it is, move to the next index
                    if (IsAtPosition(GetPosition(m_targetIndex)))
                    {
                        // if moving towards end of array
                        if (m_isActive)
                        {
                            // if not at end of array
                            if (!(m_targetIndex == m_path.Length - 1 && IsAtPosition(GetPosition(m_path.Length - 1))))
                                m_targetIndex++;
                        }
                        // if moving towards start of array
                        else
                        {
                            // if not at start of array
                            if (!(m_targetIndex == 0 && IsAtPosition(GetPosition(0))))
                                m_targetIndex--;
                        }
                    }

                    // move the object towards its target position
                    m_rb.MovePosition(Vector3.MoveTowards(transform.position, GetPosition(m_targetIndex), m_movementSpeed * Time.deltaTime));
                }

#if UNITY_EDITOR
                [ContextMenu("Reset Platform Position")]
                private void SetPlatformToInitialPosition()
                {
                    if (m_path.Length > 0)
                        transform.position = GetPosition(0);
                    else
                        Debug.LogError("The platform's path is empty!");
                }
#endif
            }
        }
    }
}
