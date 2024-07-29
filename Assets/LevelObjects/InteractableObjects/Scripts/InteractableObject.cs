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
		public abstract class InteractableObject : LevelObject
		{
			[Header("Interactable Details"), Tooltip("How long in seconds it takes for an object to be interacted with before it can be interacted with again.")]
			[SerializeField] private float m_interactTime;
			private float m_timer;
			
			protected abstract void OnInteractSucess();
			protected virtual void OnInteractFail() { }

			public void Interact()
			{
				// if the object is not currently being interacted with
				if (m_timer <= 0)
				{
					// if the object is active, do the interaction
					// otherwise, do the fail logic
					if (m_isActive)
						OnInteractSucess();
					else
						OnInteractFail();

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
