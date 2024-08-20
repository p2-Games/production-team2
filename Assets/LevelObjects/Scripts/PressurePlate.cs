///<summary>
/// Author: Halen
///
/// Stand on it to activate it.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        using Events;

        public class PressurePlate : LevelObject
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

            private void OnTriggerEnter(Collider other)
            {
                if (CanTrigger(other.gameObject))
                {
                    if (!m_isActive)
                        isActive = true;
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (CanTrigger(other.gameObject))
                {
                    if (m_isActive)
                        isActive = false;
                }
            }
        }
    }
}
