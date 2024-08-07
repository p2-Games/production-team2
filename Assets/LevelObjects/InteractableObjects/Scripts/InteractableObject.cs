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
		using Events;

		public class InteractableObject : LevelObject
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
                    foreach (Event _ev in m_events)
                        _ev.DoEvent(value);

					// save state
                    m_isActive = value;
				}
			}

			[Header("Interactable Details"), Tooltip("If true, the player is able to interact with this object directly.")]
			[SerializeField] protected bool m_playerCanInteract = true;

			[Tooltip("If true, the object cannot change from active to inactive.")]
			[SerializeField] protected bool m_staysActive;

			[Tooltip("How long in seconds it takes for an object to be able to be interacted with again after being interacted with.")]
			[SerializeField, Min(0)] protected float m_interactTime;

			[Tooltip("The events that will occur when the object's active state is changed.")]
			[SerializeField] protected Event[] m_events;

			public bool playerCanInteract => m_playerCanInteract;

			protected float m_timer;

            public virtual void Interact()
			{
				// if the object is not currently being interacted with
				if (m_timer <= 0)
				{
					// toggle the active state
					isActive = !m_isActive;

					// start the time so it cannot be interacted with immediately
                    m_timer = m_interactTime;
                }
			}

            protected virtual void Update()
            {
				if (m_timer > 0)
					m_timer -= Time.deltaTime;
            }
        }
	}
}
