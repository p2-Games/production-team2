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
				get => m_isActive;
				set => m_isActive = value;
			}

			new public string name => m_objectName;

			[SerializeField] protected string m_objectName = "";
            [SerializeField, TextArea] protected string m_description = "";

			[Tooltip("Filter for what can interact with this object. Accepts Types and Tags.")]
            [SerializeField] private string[] m_typeFilter = { "Player", typeof(LevelObject).Name };

            protected bool CanTrigger(GameObject obj)
            {
                foreach (string type in m_typeFilter)
                {
                    if (obj.GetComponent(type) || obj.tag == type)
                        return true;
                }
                return false;
            }

            // spawn parent, if it exists
            private ObjectSpawner m_parent;
			public ObjectSpawner spawnParent { get => m_parent; set => m_parent = value; }

			public void ToggleActive() => isActive = !m_isActive;
        }
	}
}
