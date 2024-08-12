///<summary>
/// Author: Halen
///
/// Base class for level objects that move on a path.
///
///</summary>

using Millivolt.Player;
using UnityEditor;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace MovingLevelObjects
        {
            public abstract class MovingLevelObject : LevelObject
            {
                [Header("Moving Object Details"), Tooltip("How fast the platform moves in units per second.")]
                [SerializeField] protected float m_movementSpeed;
                [Tooltip("The platform moves from the start of this array to the end.")]
                [SerializeField] protected Transform[] m_path;

                protected Rigidbody m_rb; new public Rigidbody rigidbody => m_rb;
                protected int m_targetIndex;
                protected Vector3 m_direction;

                protected virtual void Start()
                {
                    m_rb = GetComponent<Rigidbody>();
                }

                protected abstract void FixedUpdate();

                public void SetMovementSpeed(float value)
                {
                    m_movementSpeed = value;
                }

                // returns if this object is at the provided position
                protected bool IsAtTransform(int index)
                {
                    return Vector3.Distance(transform.position, m_path[index].position) < m_movementSpeed * Time.fixedDeltaTime;
                }

                protected Vector3 GetDirectionToTarget()
                {
                    return (m_path[m_targetIndex].position - transform.position).normalized;
                }

                [ContextMenu("Set Level Object to Start")]
                protected void SetObjectToInitialPosition()
                {
                    if (m_path.Length > 1)
                    {
                        m_targetIndex = 0;
                        transform.position = m_path[0].position;
                        m_direction = GetDirectionToTarget();
                    }
                    else
                        Debug.LogError("A moving object's path requires at least two Transforms.");
                }

                [ContextMenu("Set Level Object to End")]
                protected void SetObjectToTerminalPosition()
                {
                    if (m_path.Length > 1)
                    {
                        m_targetIndex = m_path.Length - 1;
                        transform.position = m_path[m_targetIndex].position;
                        m_direction = GetDirectionToTarget();
                    }
                    else
                        Debug.LogError("A moving object's path requires at least two Transforms.");
                }

                private void OnTriggerStay(Collider other)
                {
                    PlayerController player = other.GetComponent<PlayerController>();
                    if (player)
                        player.SetPlatformVelocity(m_rb.GetPointVelocity(other.ClosestPointOnBounds(player.transform.position)));
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    if (m_path.Length == 0)
                        return;

                    for (int t = 0; t < m_path.Length; t++)
                    {
                        if (m_path[t] == null)
                            return;

                        if (t == m_targetIndex)
                        {
                            Handles.color = Color.green;
                            Handles.DrawDottedLine(transform.position, m_path[t].position, 1);
                        }
                        else
                            Handles.color = Color.white;

                        Handles.DrawWireCube(m_path[t].position, Vector3.one / 2f);
                    }
                }
#endif
            }
        }
    }
}
