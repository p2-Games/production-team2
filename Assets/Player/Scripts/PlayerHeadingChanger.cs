///<summary>
/// Author: Halen
///
/// Changes the rotation of the player to face a specific object.
///
///</summary>

using UnityEngine;

namespace Millivolt.Player
{
	public class PlayerHeadingChanger : MonoBehaviour
	{
		[SerializeField] private Transform m_targetObject;

		public void SetPlayerHeading()
		{
			GameManager.Player.Model.SetHeading(m_targetObject.position);
		}
	}
}
