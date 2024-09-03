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
		namespace EventObjects
		{
			public class TriggerObject : EventObject
			{
                public override bool isActive
				{
					get => m_isActive;
					set => m_isActive = value;
				}

                public override void Interact()
				{
					base.Interact();

					m_activateEvents.Invoke();

					if (m_togglesOnce)
						m_canInteract = false;
				}
			}
		}
	}
}
