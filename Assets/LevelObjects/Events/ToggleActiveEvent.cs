///<summary>
/// Author: Halen
///
/// Toggles isActive on the array of connected level objects.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        using Events;

        public class ToggleActiveEvent : Event
        {
            [SerializeField] private LevelObject[] m_connectedObjects;

            public override void DoEvent(bool value)
            {
                foreach (LevelObject obj in m_connectedObjects)
                    obj.isActive = !obj.isActive;
            }
        }
    }
}
