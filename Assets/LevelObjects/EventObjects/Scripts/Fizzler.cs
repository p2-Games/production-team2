///<summary>
/// Author: Halen
///
/// Destroys objects that enter its trigger if they match the interaction filter.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace LevelObjects
	{
		namespace EventObjects
		{
			public class Fizzler : EventObject
			{
                public override bool isActive { get => m_isActive; set => m_isActive = value; }

                private void OnTriggerEnter(Collider other)
                {
					if (CanTrigger(other.gameObject))
					{
						Destroy(other.gameObject);
						m_activateEvents.Invoke();
					}
                }
            }
		}
	}
}
