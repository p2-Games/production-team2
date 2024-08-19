///<summary>
/// Author: Halen
///
/// Sets the active state of all connected Level Objects to false.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class SetInactiveEvent : Event
            {
                [SerializeField] private LevelObject[] m_connectedObjects;

                public override void DoEvent(bool value)
                {
                    foreach (LevelObject obj in m_connectedObjects)
                        obj.isActive = false;
                }
            }
        }
    }
}
