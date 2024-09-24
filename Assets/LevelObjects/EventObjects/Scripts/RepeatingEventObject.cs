///<summary>
/// Author: Halen
///
/// Triggers events repeatedly.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace LevelObjects
	{
		namespace EventObjects
		{
			public class RepeatingEventObject : EventObject
			{
                public override bool isActive
				{
					get => m_isActive;
					set
					{
						m_isActive = value;

						if (value && m_pulseImmediately)
							Pulse();
						else
                            m_timer = 0;
                    }
				}

				[Header("Repeating Event Details"), Tooltip("How often a pulse occurs in seconds.")]
                [SerializeField] private float m_delay;
				private float m_timer;

				[Tooltip("The value of the first pulse. Also tracks what the next pulse will be.")]
				[SerializeField] private bool m_pulse;

				[Tooltip("If true, a pulse will occur instantly when the object is activated.")]
				[SerializeField] private bool m_pulseImmediately;

                private void Update()
                {
					if (!m_isActive)
						return;
					
					if (m_timer < m_delay)
						m_timer += Time.deltaTime;
					else
						Pulse();
                }

				private void Pulse()
				{
					if (m_pulse)
						m_activateEvents.Invoke();
					else
						m_deactivateEvents.Invoke();

					m_pulse = !m_pulse;

					m_timer = 0;
				}
            }
		}
	}
}
