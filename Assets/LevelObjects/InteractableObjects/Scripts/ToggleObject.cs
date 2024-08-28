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
		namespace InteractableObjects
		{
			public class ToggleObject : InteractableObject
			{
                /// <summary>
                /// Used for changing the active state of the Event Object.
                /// </summary>
                /// <param name="value">The state to set the Event Object to.</param>
                /// <returns>If the active state of the Event Object was changed.</returns>
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

                [SerializeField] protected UnityEvent m_deactivateEvents;

                public override void Interact()
                {
                    // if the object is not currently being interacted with
                    if (m_interactTimer >= m_interactDelay)
                    {
                        // toggle the active state
                        isActive = !m_isActive;

                        if (m_togglesOnce)
                            m_canInteract = false;
                        else
                            m_interactTimer = 0;
                    }
                }

                public void SetActive(bool value) => isActive = value;
            }
		}
	}
}
