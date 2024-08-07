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

                [Tooltip("How long it will take before gravity changes.")]
                [SerializeField] private float m_gravityChangeTime;

                [Tooltip("The magnitude and direction of gravity when this event triggers.")]
                [SerializeField] private Vector3 m_gravity;

                private float m_timer;
                private bool m_active = false;

                public void ChangeGravityAfterTime()
                {
                    float indicatorFlashInterval = (m_gravityChangeTime / 5);
                    m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(indicatorFlashInterval));
                    m_active = true;
                }

                public void ChangeGravity()
                {
                    GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetGravity(m_gravity);
                    m_active = false;
                }

                public override void DoEvent(bool value)
                {
                    if (m_changeInstantaneously)
                        ChangeGravity();
                    else
                    {
                        ChangeGravityAfterTime();
                        m_timer = m_gravityChangeTime;
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
                    Vector3 direction = m_gravity.normalized;
                    Vector3 endPoint = transform.position + direction;
                    Handles.DrawLine(transform.position, endPoint);
                    Handles.ArrowHandleCap(0, endPoint, Quaternion.LookRotation(direction), 1, EventType.Repaint);
                }
#endif
            }
        }
    }
}
