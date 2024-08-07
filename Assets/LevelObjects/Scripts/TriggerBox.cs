///<summary>
/// Author: Emily
///
/// Enter the box to activate a function (Basically an invisible pressure plate)
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        using Events;
        public class TriggerBox : LevelObject
        {
            [SerializeField] private Event[] m_events;

            public override bool isActive
            {
                get => base.isActive;
                set
                {
                    foreach (Event ev in m_events)
                        ev.DoEvent(value);

                    m_isActive = value;
                }
            }

            private int m_collidingObjects;

            public bool CanTrigger(GameObject obj) => obj.GetComponent<Player.PlayerController>() || obj.GetComponent<LevelObject>();

            private void Start()
            {
                m_collidingObjects = 0;
            }

            private void OnTriggerEnter(Collider other)
            {
                if (CanTrigger(other.gameObject))
                {
                    if (m_collidingObjects == 0)
                        isActive = true;
                    m_collidingObjects++;
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (CanTrigger(other.gameObject))
                {
                    m_collidingObjects--;
                    if (m_collidingObjects == 0)
                        isActive = false;
                }
            }
        }
    }
}
