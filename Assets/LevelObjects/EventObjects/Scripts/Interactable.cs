///<summary>
/// Author: Halen
///
/// Base class for objects the player can interact with.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	using Player;

	namespace LevelObjects
	{
		using EventObjects;
        using Millivolt.Level;
        using PickupObjects;

        public class Interactable : MonoBehaviour
		{
			[Tooltip("How long in seconds it takes for an object to be able to be interacted with again after being interacted with.")]
			[SerializeField, Min(0)] private float m_interactDelay;
			private float m_interactTimer;

            private void Update()
            {
                if (m_interactTimer < m_interactDelay)
                    m_interactTimer += Time.deltaTime;
            }

            public bool canInteract
			{
				get
				{
                    // ensure object is ready to be interacted with.
                    if (m_interactTimer < m_interactDelay)
                        return false;

                    if (TryGetComponent(out EventObject eventObject))
                    {
						return eventObject.canInteract;
                    }

                    if (TryGetComponent(out PickupObject pickupObject))
                    {
						return pickupObject.canInteract;
                    }

					if (GetComponent<Checkpoint>())
					{ 
#if UNITY_EDITOR
						return true;
#else
						return false;
#endif
					}
					return false;
                }
			}

			public void Interact(PlayerInteraction player)
			{
				if (TryGetComponent(out EventObject eventObject))
				{
					eventObject.Interact();
					m_interactTimer = 0;
					return;
				}

				if (TryGetComponent(out PickupObject pickupObject) && pickupObject.canInteract)
				{
					player.GrabObject(pickupObject);
					m_interactTimer = 0;
                    return;
				}

                if (TryGetComponent(out Checkpoint checkpoint))
                {
                    checkpoint.Interact();
					return;
                }
            }
		}
	}
}
