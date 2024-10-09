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

			private CheckpointTeleporter m_teleporter;

			public void Initialise(int id)
			{
				m_checkpointID = id;
				m_teleporter = transform.parent.GetComponent<CheckpointTeleporter>();
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
				if (other.CompareTag("Player") && m_checkpointID != LevelManager.Instance.activeCheckpointIndex)
				{
					SetActiveCheckpoint();
				}
            }

            private void OnTriggerExit(Collider other)
            {
                m_teleporter.SetDisplayActive(false);
            }

            public void SetActiveCheckpoint()
			{
				LevelManager.Instance.activeCheckpointIndex = m_checkpointID;

				// if checkpoint is not unlocked for the teleporter, then unlock it
				if (!m_teleporter.CheckpointIsUnlocked(m_checkpointID))
					m_teleporter.UnlockCheckpoint(m_checkpointID);
            }

			public void Interact()
			{
				m_teleporter.SetDisplayActive(true);
			}
        }
	}
}
