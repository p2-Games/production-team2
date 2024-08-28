///<summary>
/// Author: Halen
///
/// Base class for objects the player can interact with.
///
///</summary>

using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Millivolt
{
	namespace LevelObjects
	{
		namespace InteractableObjects
		{
			public abstract class InteractableObject : LevelObject
			{
				[Header("Interactable Details"), Tooltip("If true, the player is able to interact with this object directly.")]
				[SerializeField] protected bool m_playerCanInteract = true;

				[Tooltip("If true, the object can only be interacted with once.")]
				[SerializeField] protected bool m_togglesOnce;

				[Tooltip("How long in seconds it takes for an object to be able to be interacted with again after being interacted with.")]
				[SerializeField, Min(0)] protected float m_interactDelay;

                [Tooltip("Filter for what can interact with this object.\n" +
					"Accepts System Types (class names) and Tags.")]
                [SerializeField] private string[] m_interactionFilter = { "Player", typeof(LevelObject).Name };

				[Tooltip("The events that will occur when the object's active state is changed.")]
				[SerializeField] protected UnityEvent m_activateEvents;

				public bool playerCanInteract => m_playerCanInteract;

				protected float m_interactTimer;

				public abstract void Interact();

                protected bool CanTrigger(GameObject obj)
                {
                    foreach (string type in m_interactionFilter)
                    {
                        if (obj.GetComponent(type) || obj.tag == type)
                            return true;
                    }
                    return false;
                }

                protected virtual void Update()
				{
					if (m_interactTimer < m_interactDelay)
						m_interactTimer += Time.deltaTime;
				}
			}
		}
	}
}
