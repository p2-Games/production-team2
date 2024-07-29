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
			[Header("Interactable Details"), Tooltip("How long in seconds it takes for an object to be interacted with before it can be interacted with again.")]
			[SerializeField] private float m_interactTime;
			[Tooltip("If true, the object cannot change from active to inactive.")]
			[SerializeField] private bool m_staysActive;
			[SerializeField] private Event[] m_events;

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
