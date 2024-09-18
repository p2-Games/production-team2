
using UnityEngine;

///<summary>
/// Author: Halen
///
/// Interactable Object that is triggered when interacted with. Effectively a toggle object that always gets set to 'true'.
///
///</summary>

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

					isActive = true;

					/*
					m_activateEvents.Invoke();

					// play sound effect if it exists
					if (m_soundClipCollectionName != string.Empty && m_activateSoundClipName != string.Empty)
						SFXController.Instance.PlaySoundClip(m_soundClipCollectionName, m_activateSoundClipName, transform);

					if (m_togglesOnce)
						m_canInteract = false;
					*/
				}
			}
		}
	}
}
