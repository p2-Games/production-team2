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
    using Millivolt.Utilities;
    using Player;

    namespace LevelObjects
    {
        namespace Events
        {
            public class GravityChanger : LevelObject
            {
                [Header("Gravity"), Tooltip("The acceleration of gravity in units per second squared.")]
                [SerializeField] private float m_magnitude;

                [Tooltip("The direction of gravity when this event triggers.")]
                [SerializeField] private Vector2 m_direction;

                private PlayerController m_player;

                /// <summary>
                /// Changes the gravity for the Player and all rigidbodies in the scene.
                /// </summary>
                /// <param name="flip">If the gravity should change to the set value or to the level default.</param>
                protected void ChangeGravity(bool flip)
                {
                    if (!m_player)
                        m_player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

                    m_player.SetGravity(flip ? m_magnitude : LevelManager.Instance.levelData.gravityMagnitude, flip ? m_direction : LevelManager.Instance.levelData.gravityDirection);
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    Handles.color = Color.yellow;
                    Vector3 direction = Quaternion.Euler(m_direction) * Vector3.forward;
                    Vector3 endPoint = transform.position + direction;
                    Handles.DrawLine(transform.position, endPoint);
                    Handles.ArrowHandleCap(0, endPoint, Quaternion.Euler(m_direction), 1, EventType.Repaint);
                }
#endif
            }
        }
    }
}
