///<summary>
/// Author: Emily McDonald
///
/// Handles the respawning of the player at a checkpoint
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace Level
    {
        public class Checkpoint : MonoBehaviour
		{
			[SerializeField] private Transform m_respawnPoint;
			public Transform respawnPoint => m_respawnPoint;

            [SerializeField] private int m_checkpointID;
            public int checkpointID { get => m_checkpointID; }

			public void Initialise(int id)
			{
				m_checkpointID = id;
			}

            private void OnEnable()
            {
				if (m_respawnPoint == null)
				{
					m_respawnPoint = transform;
				}
            }

            private void OnTriggerEnter(Collider other)
            {
				if (other.CompareTag("Player") && m_checkpointID != LevelManager.Instance.activeCheckpoint)
				{
					SetActiveCheckpoint();
				}
            }

			public void SetActiveCheckpoint()
			{
                LevelManager.Instance.activeCheckpoint = m_checkpointID;
            }
        }
	}
}
