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
			[SerializeField] public bool activeCheckpoint;

			public int checkpointID;

			private LevelData m_lvlData;

            private void OnEnable()
            {
				m_lvlData = FindAnyObjectByType<LevelData>();
				if (m_respawnPoint == null)
				{
					m_respawnPoint = transform;
				}
            }

            /// <summary>
            /// This will be called to load the player to checkpoint position
            /// </summary>
            /// <param name="player"></param>
            public void RespawnPlayer(GameObject player)
			{
				player.transform.rotation = m_respawnPoint.rotation;
				player.transform.position = m_respawnPoint.position;
			}

            private void OnTriggerEnter(Collider other)
            {
				if (other.tag == "Player" && this != m_lvlData.GetActiveCheckpoint())
				{
					Debug.Log("Checkpoint set to Checkpoint " + checkpointID);
					m_lvlData.SetActiveCheckpoint(checkpointID);
				}
            }
        }
	}
}
