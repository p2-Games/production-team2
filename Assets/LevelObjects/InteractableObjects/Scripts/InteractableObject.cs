///<summary>
/// Author: Halen
///
/// Base class for objects the player can interact with.
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
			public abstract class InteractableObject : EventObject
			{
				[Header("Interactable Object Details"), Tooltip("If true, the player is able to interact with this object directly.")]
				[SerializeField] protected bool m_playerCanInteract = true;

				[Tooltip("If true, the object cannot change from active to inactive.")]
				[SerializeField] protected bool m_staysActive;

                [Tooltip("How long in seconds it takes for an object to be able to be interacted with again after being interacted with.")]
                [SerializeField, Min(0)] protected float m_interactTime;
                protected float m_interactTimer;

				public bool playerCanInteract => m_playerCanInteract;

				public abstract void Interact();

                protected virtual void Update()
                {
                    if (m_interactTimer < m_interactTime)
                        m_interactTimer += Time.deltaTime;
                }
            }
		}
	}
}
