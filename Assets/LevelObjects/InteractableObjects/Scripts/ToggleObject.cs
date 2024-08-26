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
                public override bool isActive
                {
                    get => base.isActive;
                    set
                    {
                        // do nothing if the object should stay active and is currently active
                        if (m_staysActive && m_isActive)
                            return;

                        // if the active state is being changed, do all events
                        foreach (UnityEvent<bool> _event in m_activateEvents)
                            _event.Invoke(value);

                        // save state
                        m_isActive = value;
                    }
                }

                public override void Interact()
                {
                    // if the object is not currently being interacted with
                    if (m_interactTimer > m_interactTime)
                    {
                        // toggle the active state
                        isActive = !m_isActive;

                        // start the time so it cannot be interacted with immediately
                        m_interactTimer = 0;
                    }
                }
            }
		}
	}
}
