///<summary>
/// Author: Halen
///
/// Enables or disables a specified GameObject.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	public class ToggleGameObject : MonoBehaviour
	{
		[SerializeField] GameObject m_parentObject;

		public void ToggleObject()
		{
			m_parentObject.SetActive(!m_parentObject.activeSelf);
		}
	}
}
