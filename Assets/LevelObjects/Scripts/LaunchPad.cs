///<summary>
/// Author: Halen
///
/// Sets the velocity of the player when they enter a trigger.
///
///</summary>

using UnityEditor;
using UnityEngine;

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

            [SerializeField] private Vector3 m_launchForce;
            private Collider m_trigger;

            private void Start()
            {
                m_trigger = GetComponent<Collider>();
            }

            private void OnTriggerEnter(Collider other)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player)
                {
                    player.AddVelocity(m_launchForce);
                }
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                Handles.color = Color.blue;
                Handles.DrawLine(transform.position, transform.position + m_launchForce.normalized, m_launchForce.magnitude);
            }
#endif
        }
    }
}
