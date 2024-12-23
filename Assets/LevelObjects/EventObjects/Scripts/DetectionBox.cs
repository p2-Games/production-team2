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
        namespace EventObjects
        {
            public class DetectionBox : EventObject
            {
                private int m_collidingObjects;

                private void OnEnable()
                {
                    m_collidingObjects = 0;
                }

                private void OnTriggerEnter(Collider other)
                {
                    if (!m_canInteract)
                        return;

                    if (CanTrigger(other.gameObject))
                    {
                        if (m_collidingObjects == 0)
                            isActive = true;
                        m_collidingObjects++;
                    }
                }

                private void OnTriggerExit(Collider other)
                {
                    if (!m_canInteract)
                        return;

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
}
