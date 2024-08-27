///<summary>
/// Author: Emily McDonald & Halen
///
/// Changes player up transform to a specific vector3 when event is called
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
    using Utilities;
    using Player;

    namespace LevelObjects
    {
        namespace Events
        {
            public class GravityChanger : LevelObject
            {
                private PlayerController m_player;

                [Header("Gravity"), Tooltip("The acceleration of gravity in units per second squared.")]
                [SerializeField] private float m_magnitude;

                [Tooltip("The euler direction of gravity when this event triggers.")]
                [SerializeField] private Vector3 m_direction;

                [Tooltip("If this object changes the value of Physics.gravity as well as the player's personal gravity.")]
                [SerializeField] private bool m_changePhysicsGravity;

                public void ChangeGravity(bool value)
                {
                    float magnitude = value ? m_magnitude : LevelManager.Instance.currentLevelData.gravityMagnitude;
                    Vector3 direction = value ? m_direction : LevelManager.Instance.currentLevelData.gravityEulerDirection;

                    if (!m_player)
                        m_player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

                    m_player.SetGravity(magnitude, direction); 
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    Handles.color = Color.yellow;
                    //Vector3 direction = Quaternion.Euler(m_direction) * Vector3.up / 2f;
                    //Vector3 endPoint = transform.position + direction;
                    //Handles.DrawLine(transform.position, endPoint);
                    Handles.ArrowHandleCap(0, transform.position, Quaternion.Euler(m_direction) * Quaternion.FromToRotation(Vector3.forward, Vector3.down), 1.4f, EventType.Repaint);
                }
#endif
            }
        }
    }
}
