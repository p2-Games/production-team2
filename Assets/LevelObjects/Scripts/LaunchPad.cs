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

            private PlayerController m_player;
            private Collider m_trigger;

            private void Start()
            {
                m_trigger = GetComponent<Collider>();
                m_player = FindObjectOfType<PlayerController>();
            }

            private void OnTriggerEnter(Collider other)
            {;
                if (other.gameObject == m_player.gameObject)
                {
                    Vector3 newPlayerPosition = transform.position;
                    newPlayerPosition.y += m_player.height / 2;
                    m_player.transform.position = newPlayerPosition;
                    m_player.SetExternalVelocity(m_initialVelocity);
                }
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (!m_player)
                    m_player = FindObjectOfType<PlayerController>();

                Handles.color = Color.blue;

                Vector3 lastPosition = transform.position;

                for (int p = 1; p < m_debugPointsToDraw; p++)
                {
                    float t = p * m_debugTimeBetweenPoints;
                    Vector3 nextPoint = m_initialVelocity;
                    nextPoint *= t;
                    nextPoint.y = m_initialVelocity.y * t + 0.5f * m_player.gravity * t * t;

                    Vector3 position = transform.position + nextPoint;

                    if (Physics.Raycast(lastPosition, (position - lastPosition).normalized, out RaycastHit hit, Vector3.Distance(lastPosition, position), ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore))
                    {
                        if (m_drawLines)
                            Handles.DrawLine(lastPosition, hit.point);
                        Handles.color = Color.red;
                        Handles.DrawWireCube(hit.point, new Vector3(0.5f, 0.5f, 0.5f));
                        break;
                    }
                    else
                    {
                        if (m_drawLines)
                            Handles.DrawLine(lastPosition, position);
                        else
                            Handles.DrawWireCube(position, new Vector3(0.5f, 0.5f, 0.5f));
                    }

                    lastPosition = position;
                }
            }
#endif
        }
    }
}
