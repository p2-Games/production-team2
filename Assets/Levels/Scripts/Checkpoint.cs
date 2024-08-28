///<summary>
/// Author: Emily McDonald
///
/// Handles the respawning of the player at a checkpoint
///
///</summary>

using Cinemachine;
using Millivolt.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Millivolt
{
	namespace Utilities
	{
		public class Checkpoint : MonoBehaviour
		{
			[SerializeField] private Transform m_respawnPoint;
			public Transform respawnPoint => m_respawnPoint;
			[SerializeField] public bool activeCheckpoint;

            [SerializeField] private int m_checkpointID;
            public int checkpointID
			{
				get => m_checkpointID;
				set { m_checkpointID = value; }
			}

			private LevelManager m_levelManager;

			private PlayerController m_player;

            private void OnEnable()
            {
				m_levelManager = FindObjectOfType<LevelManager>();
				m_player = FindObjectOfType<PlayerController>();
				if (m_respawnPoint == null)
				{
					m_respawnPoint = transform;
				}
            }

            private void OnTriggerEnter(Collider other)
            {
				if (other.tag == "Player" && this != m_levelManager.GetActiveCheckpoint())
				{
					Debug.Log("Checkpoint set to Checkpoint " + checkpointID);
					m_levelManager.SetActiveCheckpoint(checkpointID);
				}
            }
        }
	}
}
