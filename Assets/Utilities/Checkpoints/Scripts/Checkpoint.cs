///<summary>
/// Author: Emily McDonald
///
/// Handles the respawning of the player at a checkpoint
///
///</summary>

using Cinemachine;
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

			private LevelData m_lvlData;

			[SerializeField] private Vector2 m_respawnCamDir;

            [SerializeField] private CinemachineVirtualCamera m_fpsCam;

            private void OnEnable()
            {
				m_lvlData = FindObjectOfType<LevelData>();
				m_fpsCam = FindObjectOfType<CinemachineVirtualCamera>();
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
				player.transform.position = m_respawnPoint.position;
				//Add Logic for turning the player to the rotation of the checkpoint


				//SceneManager.LoadScene("[MicrowavePrototyping01]");
                //CinemachinePOV pov = m_fpsCam.GetCinemachineComponent<CinemachinePOV>();
                //pov.m_VerticalAxis.Value = m_respawnCamDir.x;
                //pov.m_HorizontalAxis.Value = m_respawnCamDir.y;
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
