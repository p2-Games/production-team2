///<summary>
/// Author: Emily McDonald
///
/// Handles the management of the level
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    using Millivolt.Player;
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

			[SerializeField] public int activeCheckpointIndex
			{
				get { return m_activeCheckpointIndex; }
				set
				{
					m_levelCheckpoints[m_activeCheckpointIndex].DisableCheckpoint();
					m_levelCheckpoints[value].EnableCheckpoint();
					m_activeCheckpointIndex = value;
				}
			}
			private int m_activeCheckpointIndex;

			private List<Checkpoint> m_levelCheckpoints;

            //Bool to check if its the first time loading into the level
            private bool m_firstCheckpointInit = true;

            public Checkpoint activeCheckpoint { get { return m_levelCheckpoints[activeCheckpointIndex]; } }
			public int checkpointCount { get { return m_levelCheckpoints.Count; } }

			// Level Data
			[SerializeField] private LevelData m_levelData;
			public LevelData levelData => m_levelData;

			// Timer Details
			private float m_levelTimer;
			public float levelTimer => m_levelTimer;

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
				GameManager.Player.Controller.ResetPlayer();
				GameManager.Player.Model.StopAllCoroutines();

                GameManager.Instance.SetGravity(levelData.gravityDirection, levelData.gravityMagnitude);

				GameManager.Player.Controller.transform.position = activeCheckpoint.respawnPoint.position;
				GameManager.Player.Model.SetHeading(activeCheckpoint.frontPosition);
				//GameManager.PlayerModel.transform.rotation = Quaternion.LookRotation(activeCheckpoint.transform.forward, -Physics.gravity.normalized);

				UIMenuManager.Instance.CursorLockupdate();

                if (m_firstCheckpointInit)
                {
                    FindAllCheckpoints();
                    InitialiseCheckpoints();
                    m_firstCheckpointInit = false;
                }
			}

            private void Update()
            {
				m_levelTimer += Time.deltaTime;
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
				m_levelTimer = 0;
				PlayerRespawn.Instance.StartRespawn(activeCheckpoint.transform.position + new Vector3(0, 0.6f, 0));
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
