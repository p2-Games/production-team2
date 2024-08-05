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
			[Header("Interactable Details"), Tooltip("If true, the player is able to interact with this object directly.")]
			[SerializeField] private bool m_playerCanInteract = true;
			[Tooltip("If true, the object cannot change from active to inactive.")]
			[SerializeField] private bool m_staysActive;
			[Tooltip("How long in seconds it takes for an object to be interacted with before it can be interacted with again.")]
			[SerializeField, Min(0)] private float m_interactTime;
			[Tooltip("The events that will occur when the object's active state is changed.")]
			[SerializeField] private Event[] m_events;

			public bool playerCanInteract => m_playerCanInteract;

			private float m_timer;

            public override bool isActive
			{   
				get => base.isActive;
				set
				{
                    foreach (Event _ev in m_events)
                        _ev.DoEvent(value);

                    m_isActive = value;
				}
			}

            private void Start()
            {
				m_isActive = isActive;
            }

            public void Interact()
			{
				// if the object is not currently being interacted with
				if (m_timer <= 0)
				{
					// only toggle if the object should not stay active
					// OR if the object should stay active and is currently inactive
					if ((m_staysActive && !m_isActive) || !m_staysActive)
						isActive = !m_isActive;
                    m_timer = m_interactTime;
                }
			}

            private void Update()
            {
				m_timer -= Time.deltaTime;
            }
        }
	}
}
