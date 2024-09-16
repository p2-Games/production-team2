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
			public Transform respawnPoint => m_respawnPoint;
			[SerializeField] public bool activeCheckpoint;

            [SerializeField] private int m_checkpointID;
            public int checkpointID
			{
				get => m_checkpointID;
				set { m_checkpointID = value; }
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
				if (other.tag == "Player" && this != GameManager.LevelManager.GetActiveCheckpoint())
				{
					Debug.Log("Checkpoint set to Checkpoint " + checkpointID);
                    GameManager.LevelManager.SetActiveCheckpoint(checkpointID);
				}
            }
        }
	}
}
