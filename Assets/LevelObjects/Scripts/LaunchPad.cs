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
using UnityEngine.ProBuilder.MeshOperations;

namespace Millivolt
{
    using PlayerController = Player.PlayerController;

    namespace LevelObjects
    {
        public class LaunchPad : LevelObject
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

            [Header("Debug"), SerializeField] private bool m_drawLines;
            [SerializeField] private int m_debugPointsToDraw;
            [SerializeField] private float m_debugTimeBetweenPoints;

            private Collider m_trigger;

            private void Start()
            {
                m_trigger = GetComponent<Collider>();
            }

            private Quaternion PlayerVerticalRotation(PlayerController player)
            {
                Vector3 euler = player.parent.rotation.eulerAngles;
                euler.y = 0;
                return Quaternion.Inverse(Quaternion.Euler(euler));
            }

            private void OnTriggerEnter(Collider other)
            {;
                if (CanTrigger(other.gameObject))
                {
                    Vector3 newObjectPosition = transform.position;
                    newObjectPosition.y += other.gameObject.GetComponent<Collider>().bounds.extents.y;
                    other.gameObject.transform.position = newObjectPosition;

                    PlayerController player = other.GetComponent<PlayerController>();
                    if (player)
                        player.SetExternalVelocity(m_initialVelocity);   
                }
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

                Handles.color = Color.blue;

                Vector3 lastPoint = transform.position;
                Vector3 velocity = m_initialVelocity;

                for (int p = 1; p < m_debugPointsToDraw; p++)
                {
                    float t = p * m_debugTimeBetweenPoints;

                    Vector3 nextPoint = transform.position + velocity;

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

                    velocity += player.gravity;
                    lastPoint = nextPoint;
                }
            }
#endif
        }
    }
}
