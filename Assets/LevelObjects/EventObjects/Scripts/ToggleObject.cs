///<summary>
/// Author: Halen
///
/// Interactable Object that can be toggled between an active and inactive state.
///
///</summary>

using UnityEngine;
using UnityEngine.Events;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace EventObjects
        {
            public class ToggleObject : EventObject
            {
                public override bool isActive
                {
                    get { return m_isActive; }
                    set
                    {
                        if (value)
                            m_activateEvents.Invoke();
                        else
                            m_deactivateEvents.Invoke();

                        if (m_togglesOnce)
                            m_canInteract = false;

                        m_isActive = value;
                    }
                }

                public override void Interact()
                {
                    base.Interact();

                    isActive = !m_isActive;
                }
            }
        }
    }
}
