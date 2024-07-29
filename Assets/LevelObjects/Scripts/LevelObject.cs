///<summary>
/// Author: Halen
///
/// Base class for any non-standard objects that appear within levels.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace LevelObjects
	{
		public abstract class LevelObject : MonoBehaviour
		{
			[SerializeField] protected bool m_isActive;

			/// <summary>
			/// Used for changing the active state of the Level Object.
			/// </summary>
			/// <param name="value">The state to set the Level Object to.</param>
			/// <returns>If the active state of the Level Object was changed.</returns>
			public virtual bool isActive
			{
				get { return m_isActive; }
				set { m_isActive = value; }
			}

			public bool CanTrigger(GameObject obj) => obj.GetComponent<Player.PlayerController>() || obj.GetComponent<LevelObject>();

#if UNITY_EDITOR
            [SerializeField, TextArea] protected string m_description = "";
#endif

            private void Start()
            {
				isActive = m_isActive;
            }
        }
	}
}
