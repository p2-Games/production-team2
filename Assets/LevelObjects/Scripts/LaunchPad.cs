///<summary>
/// Author: Halen
///
/// Sets the velocity of the player when they enter a trigger.
///
///</summary>

using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;

namespace Millivolt
{
    using PlayerController = Player.PlayerController;

    namespace LevelObjects
    {
        public class LaunchPad : EventObject
        {
            public override bool isActive
            {
                get => base.isActive;
                set
                {
                    m_trigger.enabled = value;
                    m_isActive = value;
                }
            }

            [SerializeField] private Vector3 m_initialVelocity;

            // moving object to centre
            [SerializeField] private float m_snapSpeed;
            private Rigidbody m_toBeBounced;
            private Vector3 m_snapTargetPosition;

            [Header("Debug"), SerializeField] private bool m_drawLines;
            [SerializeField] private int m_debugPointsToDraw;
            [SerializeField] private float m_debugTimeBetweenPoints;

            private Collider m_trigger;

            private void Start()
            {
                m_trigger = GetComponent<Collider>();
            }

            private void FixedUpdate()
            {
                if (!m_toBeBounced)
                    return;

                // move towards the bounce point
                m_toBeBounced.MovePosition(Vector3.MoveTowards(m_toBeBounced.position, m_snapTargetPosition, m_snapSpeed * Time.fixedDeltaTime));
            }

            private void Update()
            {
                // once object is close enough, bounce them
                if (m_toBeBounced && Vector3.Distance(m_toBeBounced.position, m_snapTargetPosition) < 0.05f)
                {
                    foreach (UnityEvent<bool> _event in m_activateEvents)
                        _event.Invoke(true);
                    
                    PlayerController player = m_toBeBounced.GetComponent<PlayerController>();
                    if (player)
                        player.SetExternalVelocity(m_initialVelocity);
                    else
                        m_toBeBounced.velocity = m_initialVelocity;
                    m_toBeBounced = null;
                }
            }

            private void OnTriggerEnter(Collider other)
            {;
                if (!m_toBeBounced && CanTrigger(other.gameObject) && other.GetComponent<Rigidbody>())
                {
                    m_snapTargetPosition = transform.position;
                    m_snapTargetPosition.y += other.GetComponent<Collider>().bounds.extents.y;
                    m_toBeBounced = other.GetComponent<Rigidbody>();

                    PlayerController player = m_toBeBounced.GetComponent<PlayerController>();
                    if (player)
                        player.canMove = false;
                }
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

                Vector3 lastPoint = transform.position;

                Handles.color = Color.blue;

                for (int p = 1; p < m_debugPointsToDraw; p++)
                {
                    float t = p * m_debugTimeBetweenPoints;

                    Vector3 nextPoint = transform.position + m_initialVelocity * t + 0.5f * player.gravity * t * t;

                    if (Physics.Raycast(lastPoint, (nextPoint - lastPoint).normalized, out RaycastHit hit, Vector3.Distance(lastPoint, nextPoint), ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore))
                    {
                        if (m_drawLines)
                            Handles.DrawLine(lastPoint, hit.point);
                        Handles.color = Color.red;
                        Handles.DrawWireCube(hit.point, new Vector3(0.5f, 0.5f, 0.5f));
                        break;
                    }
                    else
                    {
                        if (m_drawLines)
                            Handles.DrawLine(lastPoint, nextPoint);
                        else
                            Handles.DrawWireCube(nextPoint, new Vector3(0.5f, 0.5f, 0.5f));
                    }
;
                    lastPoint = nextPoint;
                }
            }
#endif
        }
    }
}
