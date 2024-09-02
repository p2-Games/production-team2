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
					if (!m_canInteract)
						return;

					if (m_interactTimer >= m_interactDelay)
					{
						m_activateEvents.Invoke();

						if (m_togglesOnce)
							m_canInteract = false;
						else
							m_interactTimer = 0;
					}
                }
            }
		}
	}
}
