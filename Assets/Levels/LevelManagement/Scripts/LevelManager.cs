///<summary>
/// Author: Emily McDonald
///
/// Handles the management of the level
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    using Millivolt.UI;
    using System.Collections.Generic;
    using UnityEditor;

    namespace Level
	{
		public class LevelManager : MonoBehaviour
		{
			public static LevelManager Instance;
			private void Awake()
			{
				if (Instance)
					Destroy(Instance.gameObject);
				Instance = this;
			}

			[SerializeField] public int activeCheckpointIndex;
			private List<Checkpoint> m_levelCheckpoints;

            //Bool to check if its the first time loading into the level
            private bool m_firstCheckpointInit = true;

            public Checkpoint activeCheckpoint { get { return m_levelCheckpoints[activeCheckpointIndex]; } }
			public int checkpointCount { get { return m_levelCheckpoints.Count; } }

			// Level Data
			[SerializeField] private LevelData m_levelData;
			public LevelData levelData => m_levelData;

			/// <summary>
			/// Will search through the level for any checkpoints and add them to the array
			/// </summary>
			[ContextMenu("Find Checkpoints")]
			private void FindAllCheckpoints()
			{
				Transform checkpointParent = GameObject.FindWithTag("Checkpoints").transform;
				if (checkpointParent.childCount == 0)
				{
					Debug.Log("No checkpoints found in scene!");
					return;
				}

				m_levelCheckpoints = new List<Checkpoint>();
				foreach (Transform child in checkpointParent)
					m_levelCheckpoints.Add(child.GetComponent<Checkpoint>());
			}

			/// <summary>
			/// Give all checkpoints in the level an ID
			/// </summary>
			private void InitialiseCheckpoints()
			{
				for (int id = 0; id < m_levelCheckpoints.Count; id++)
				{
					m_levelCheckpoints[id].Initialise(id);
				}
				activeCheckpointIndex = 0;
			}

			public void SpawnPlayer()
			{
				GameManager.PlayerController.ResetPlayer();
				GameManager.PlayerModel.ResetRotation();
				GameManager.PlayerInteraction.ResetInteraction();
				GameManager.PlayerStatus.ResetStatus();

                GameManager.Instance.ChangeGravity(levelData.gravityDirection, levelData.gravityMagnitude);

				GameManager.PlayerController.transform.position = m_levelCheckpoints[activeCheckpointIndex].respawnPoint.position;
				//GameManager.PlayerModel.transform.rotation = Quaternion.LookRotation(activeCheckpoint.transform.forward, -Physics.gravity.normalized);

				UIMenuManager.Instance.CursorLockupdate();

                if (m_firstCheckpointInit)
                {
                    FindAllCheckpoints();
                    InitialiseCheckpoints();
                    m_firstCheckpointInit = false;
                }
			}

			public Checkpoint GetCheckpoint(int index)
			{
				if (index < 0 || index >= m_levelCheckpoints.Count)
				{
					Debug.LogError("Checkpoint at index " + index + " does not exist.");
					return null;
				}

				return m_levelCheckpoints[index];
			}

			public void LevelSetup()
			{
                FindAllCheckpoints();
                InitialiseCheckpoints();
                SpawnPlayer();
            }

#if UNITY_EDITOR
			[SerializeField] private bool m_drawGizmos = true;

            private void OnDrawGizmos()
            {
				if (!m_drawGizmos)
					return;

				if (levelData)
				{
					Handles.color = Color.yellow;
                    Handles.ArrowHandleCap(0, transform.position, Quaternion.Euler(levelData.gravityDirection) * Quaternion.FromToRotation(Vector3.forward, Vector3.up), 1.4f, EventType.Repaint);
                }
            }
#endif
        }
	}
	
}
