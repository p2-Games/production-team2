///<summary>
/// Author: Emily McDonald
///
/// Handles the management of the level
///
///</summary>

using UnityEngine;

namespace Millivolt
{
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

			[Header("Scene Properties")]
			[HideInInspector, SerializeField] private string m_prevLevelName;
			[HideInInspector, SerializeField] private string m_nextLevelName;

			public string prevLevelName => m_prevLevelName;
			public string nextLevelName => m_nextLevelName;

			[SerializeField] public int activeCheckpointIndex;
			[SerializeField] private List<Checkpoint> m_levelCheckpoints;

			public Checkpoint activeCheckpoint { get { return m_levelCheckpoints[activeCheckpointIndex]; } }

			// Level Data
			[SerializeField] private LevelData m_levelData;
			public LevelData levelData => m_levelData;

			//Spawn screen ref
			[SerializeField] private GameObject m_spawnScreen;

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
				m_spawnScreen.SetActive(true);

				GameManager.PlayerController.ResetPlayer();
				GameManager.PlayerModel.ResetRotation();
				GameManager.PlayerInteraction.ResetInteraction();
				GameManager.PlayerStatus.ResetStatus();

                GameManager.Instance.ChangeGravity(levelData.gravityDirection, levelData.gravityMagnitude);

				GameManager.PlayerController.transform.position = m_levelCheckpoints[activeCheckpointIndex].respawnPoint.position;
				//GameManager.PlayerModel.transform.rotation = Quaternion.LookRotation(activeCheckpoint.transform.forward, -Physics.gravity.normalized);

                FindAllCheckpoints();
				InitialiseCheckpoints();
			}

			public void Reload()
			{
                if (m_levelCheckpoints.Count == 0)
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
