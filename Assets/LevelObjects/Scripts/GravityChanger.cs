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
    namespace LevelObjects
    {
        namespace Events
        {
            public class GravityChanger : MonoBehaviour
            {
                [Tooltip("The acceleration of gravity in units per second squared.")]
                [SerializeField] private float m_magnitude;

                [Tooltip("The direction of gravity when this event triggers.")]
                [SerializeField] private Vector3 m_direction;

                /// <summary>
                /// Changes the gravity for the Player and all rigidbodies in the scene.
                /// </summary>
                [ContextMenu("Set Physics.gravity")]
                public void ChangeGravity()
                {
                    GameManager.Instance.ChangeGravity(m_direction, m_magnitude);
                    SFXController.Instance.PlayRandomSoundClip("GravitySwitch", GameManager.PlayerController.transform.position);
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    Handles.color = Color.yellow;
                    Handles.ArrowHandleCap(0, transform.position, Quaternion.Euler(m_direction) * Quaternion.FromToRotation(Vector3.forward, Vector3.up), 1.4f, EventType.Repaint);
                }
#endif
            }
        }
    }
}
