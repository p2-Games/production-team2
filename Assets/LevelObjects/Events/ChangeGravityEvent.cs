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

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class ChangeGravityEvent : Event
            {
                [SerializeField] private GravityIndicatorUI m_gravityUI;

                [Tooltip("If true, the event will change the direction and magnitude of gravity instantly.")]
                [SerializeField] private bool m_changeInstantaneously;

                [Tooltip("How long it will take before gravity changes if change instantaneously is false.")]
                [SerializeField] private float m_changeTime;

                [Header("Gravity"), Tooltip("The acceleration of gravity in units per second squared.")]
                [SerializeField] private float m_gravityMagnitude;

                [Tooltip("The direction of gravity when this event triggers.")]
                [SerializeField] private Vector3 m_gravityEulerDirection;

                private float m_timer;
                private bool m_active = false;

                public void ChangeGravityAfterTime()
                {
                    float indicatorFlashInterval = (m_changeTime / 5);
                    m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(indicatorFlashInterval));
                    m_active = true;
                }

                public void ChangeGravity()
                {
                    GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetGravity(m_gravityMagnitude, m_gravityEulerDirection);
                    m_active = false;
                }

                public override void DoEvent(bool value)
                {
                    if (m_changeInstantaneously)
                        ChangeGravity();
                    else
                    {
                        ChangeGravityAfterTime();
                        m_timer = m_changeTime;
                    }
                }

                private void Update()
                {
                    if (m_timer > 0)
                        m_timer -= Time.deltaTime;
                    else if (m_active)
                        ChangeGravity();
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
