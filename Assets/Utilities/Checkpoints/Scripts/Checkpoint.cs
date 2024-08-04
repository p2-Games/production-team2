///<summary>
/// Author: Emily McDonald
///
/// Handles the respawning of the player at a checkpoint
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace Utilities
	{
		public class Checkpoint : MonoBehaviour
		{
			[SerializeField] private Transform m_respawnPoint;

			public void RespawnPlayer(GameObject player)
			{
				//WOW SO COMPLEX AHAHAHAHAHAHAHAH
				player.transform.position = m_respawnPoint.position;
			}
		}
	}
}
