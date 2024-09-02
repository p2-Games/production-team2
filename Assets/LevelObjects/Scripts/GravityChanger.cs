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
                [SerializeField] private Vector3 m_direction;

                [Tooltip("If this object changes the value of Physics.gravity as well as the player's personal gravity.")]
                [SerializeField] private bool m_changePhysicsGravity;

                private PlayerController m_player;

                /// <summary>
                /// Changes the gravity for the Player and all rigidbodies in the scene.
                /// </summary>
                /// <param name="flip">If the gravity should change to the set value or to the level default.</param>
                public void ChangeGravity(bool flip)
                {
                    if (!m_player)
                        m_player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

                    m_player.SetGravity(flip ? m_magnitude : LevelManager.Instance.levelData.gravityMagnitude, flip ? m_direction : LevelManager.Instance.levelData.gravityDirection);
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    Handles.color = Color.yellow;
                    Handles.ArrowHandleCap(0, transform.position, Quaternion.Euler(m_direction) * Quaternion.FromToRotation(Vector3.forward, Vector3.down), 1.4f, EventType.Repaint);
                }
#endif
            }
        }
    }
}
