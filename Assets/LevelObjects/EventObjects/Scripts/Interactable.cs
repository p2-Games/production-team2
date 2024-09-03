///<summary>
/// Author: Halen
///
/// Base class for objects the player can interact with.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace LevelObjects
	{
		using EventObjects;

		[RequireComponent(typeof(LevelObject))]
        public class Interactable : MonoBehaviour
		{
			[Tooltip("How long in seconds it takes for an object to be able to be interacted with again after being interacted with.")]
			[SerializeField, Min(0)] private float m_interactDelay;
			private float m_interactTimer;
			
			public bool canInteract
			{
				get
				{
                    EventObject eventObject = GetComponent<EventObject>();
                    if (eventObject)
                    {
						return eventObject.canInteract;
                    }

                    PickupObject pickupObject = GetComponent<PickupObject>();
                    if (pickupObject)
                    {
						return pickupObject.playerCanGrab;
                    }

					Debug.LogError("The Interactable component on " + name + " does not have an EventObject or PickupObject.");
					return false;
                }
			}

			public void Interact(Player.PlayerInteraction player)
			{
				// ensure object is ready to be interacted with.
				if (m_interactTimer < m_interactDelay)
					return;
				
				EventObject eventObject = GetComponent<EventObject>();
				if (eventObject)
				{
					eventObject.Interact();
					m_interactTimer = 0;
					return;
				}

				PickupObject pickupObject = GetComponent<PickupObject>();
				if (pickupObject && pickupObject.playerCanGrab)
				{
					player.GrabObject(pickupObject);
					m_interactTimer = 0;
                    return;
				}
			}

			protected virtual void Update()
			{
				if (m_interactTimer < m_interactDelay)
					m_interactTimer += Time.deltaTime;
			}
		}
	}
}
