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
			[SerializeField] private GameObject m_particle;
            [SerializeField] private int m_checkpointID;

			public Transform respawnPoint => m_respawnPoint;
            public int checkpointID { get => m_checkpointID; }

			private CheckpointTeleporter m_teleporter;
			private Animator m_animator;

			public void Initialise(int id)
			{
				m_checkpointID = id;
				m_teleporter = transform.parent.GetComponent<CheckpointTeleporter>();
				m_animator = GetComponentInChildren<Animator>();
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
                m_teleporter?.SetDisplayActive(false);
            }

            public void SetActiveCheckpoint()
			{
				LevelManager.Instance.activeCheckpointIndex = m_checkpointID;

				// if checkpoint is not unlocked for the teleporter, then unlock it
				if (m_teleporter && !m_teleporter.CheckpointIsUnlocked(m_checkpointID))
					m_teleporter.UnlockCheckpoint(m_checkpointID);
            }

			public void Interact()
			{
				m_teleporter?.SetDisplayActive(true);
			}

			public void EnableCheckpoint()
			{
                m_animator.Play("OnActivate");
				m_particle.SetActive(true);
            }

            public void DisableCheckpoint()
			{
				m_animator.Play("OnDeactivate");
			}
        }
	}
}
