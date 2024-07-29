///<summary>
/// Author: Halen
///
/// Sets isActive to true on the array of connected level objects.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class SetActiveEvent : Event
            {
                [SerializeField] private LevelObject[] m_connectedObjects;

                public override void DoEvent(bool value)
                {
                    foreach (LevelObject obj in m_connectedObjects)
                        obj.isActive = true;
                }
            }
        }
    }
}
