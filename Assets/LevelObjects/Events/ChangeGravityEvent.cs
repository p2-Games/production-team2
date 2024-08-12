///<summary>
/// Author: Emily McDonald & Halen
///
/// Changes player up transform to a specific vector3 when event is called
///
///</summary>

using Millivolt.Player;
using Millivolt.UI;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class ChangeGravityEvent : Event
            {
                [Header("Gravity"), Tooltip("The acceleration of gravity in units per second squared.")]
                [SerializeField] private float m_gravityMagnitude;

                [Tooltip("The direction of gravity when this event triggers.")]
                [SerializeField] private Vector2 m_gravityEulerDirection;

                protected Vector2 FlippedEulerDirection(Vector2 direction) => new Vector2(-direction.x, direction.y + 180);

                protected void ChangeGravity(bool flip)
                {
                    Vector2 holdLookDir = GameObject.FindWithTag("Player").GetComponentInParent<FirstPersonCameraController>().targetRotation;
                    if (flip)
                    {                        
                        GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetGravity(m_gravityMagnitude, m_gravityEulerDirection); 
                        GameObject.FindWithTag("Player").GetComponentInParent<FirstPersonCameraController>().SetLookRotation(holdLookDir.x + 180, -holdLookDir.y);
                    }
                    else
                    {                  
                        GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetGravity(m_gravityMagnitude, FlippedEulerDirection(m_gravityEulerDirection));
                        GameObject.FindWithTag("Player").GetComponentInParent<FirstPersonCameraController>().SetLookRotation(holdLookDir.x + 180, -holdLookDir.y);
                    }
                }

                public override void DoEvent(bool value)
                {
                    ChangeGravity(value);
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    Handles.color = Color.yellow;
                    Vector3 direction = Quaternion.Euler(m_gravityEulerDirection) * Vector3.forward;
                    Vector3 endPoint = transform.position + direction;
                    Handles.DrawLine(transform.position, endPoint);
                    Handles.ArrowHandleCap(0, endPoint, Quaternion.Euler(m_gravityEulerDirection), 1, EventType.Repaint);
                }
#endif
            }
        }
    }
}
