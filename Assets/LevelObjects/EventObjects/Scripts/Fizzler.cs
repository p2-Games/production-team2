///<summary>
/// Author: Halen
///
/// Destroys objects that enter its trigger if they match the interaction filter.
///
///</summary>

using Millivolt.LevelObjects.PickupObjects;
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
						if (other.TryGetComponent(out PickupObject pickup))
							pickup.Destroy();
						else
							Destroy(other.gameObject);
						m_activateEvents.Invoke();
					}
                }
            }
		}
	}
}
