///<summary>
/// Author: Halen
///
/// Interactable Object that is triggered when interacted with. Effectively a toggle object that always gets set to 'true'.
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
			public class TriggerObject : InteractableObject
			{			
                public override void Interact()
                {
					if (!m_playerCanInteract)
						return;
					
					if (m_isActive && m_staysActive)
						return;

					if (m_interactTimer >= m_interactTime)
					{
						foreach (UnityEvent<bool> _event in m_activateEvents)
							_event.Invoke(true);

						m_interactTimer = 0;
					}

					m_isActive = true;
                }
            }
		}
	}
}
