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
			[SerializeField] public bool activeCheckpoint;

            public int checkpointID;

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

            /// <summary>
            /// This will be called to load the player to checkpoint position
            /// </summary>
            /// <param name="player"></param>
            public void RespawnPlayer()
			{
				//player.transform.position = m_respawnPoint.position;
				//SceneManager.LoadScene(SceneManager.GetActiveScene().name);

				//MAKE PLAYER GO TO THE POSITION OF THE CHECKPOINT
				//ROTATE PLAYER TO THE DESIRED POSITION
				//CALL THE LEVELMANAGER ONSPAWN FUNCTION

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
