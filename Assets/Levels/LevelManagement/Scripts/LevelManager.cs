///<summary>
/// Author: Emily McDonald
///
/// Handles the management of the level
///
///</summary>

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Millivolt.UI;
using Millivolt.Player;

namespace Millivolt
{
    using Millivolt.Player.UI;
    using System.Collections.Generic;
    using Utilities;
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

        //[Header("Checkpoint Properties")]
		[SerializeField] public int currentCheckpoint;
		[SerializeField] private List<Checkpoint> m_levelCheckpoints;
		[Tooltip("This will find all the checkpoints in the scene and add them to the list automatically on play.\nWILL NOT SORT PROPERLY")]
		[SerializeField] private bool m_autoAddCheckpoints;

		[Header("Level Data")]
		[SerializeField] private LevelData m_levelData;
		public LevelData levelData => m_levelData;
			
		//Spawn screen ref
		[SerializeField] private GameObject m_spawnScreen;


		//public PlayerController m_player;

        private void Start()
        {
			m_spawnScreen = FindObjectOfType<PlayerSpawnUI>().gameObject;
			if (m_autoAddCheckpoints)
				FindAllCheckpoints();
			InitialiseCheckpoints();
			SpawnPlayer();
        }

            

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
			int id = 0;
			foreach (Checkpoint point in m_levelCheckpoints)
			{
				point.checkpointID = id;
				id++;
			}
			m_levelCheckpoints[0].activeCheckpoint = true;
		}

		/// <summary>
		/// Sets a specific checkpoint to active by its id and then sets the rest inactive
		/// </summary>
		/// <param name="id"></param>
        public void SetActiveCheckpoint(int id)
		{
			currentCheckpoint = id;
			foreach (Checkpoint point in m_levelCheckpoints)
			{
				if (point.checkpointID == id)
				{
					point.activeCheckpoint = true;
				}
				else
				{
					point.activeCheckpoint = false;
				}
			}
		}

		public void SpawnPlayer()
		{
			m_spawnScreen.SetActive(true);
			GameManager.PlayerController.transform.position = GetActiveCheckpoint().respawnPoint.position;
            GameManager.PlayerController.transform.localEulerAngles = new Vector3(0, GetActiveCheckpoint().respawnPoint.localEulerAngles.y, 0);

			GameManager.PlayerController.ResetPlayer();
			GameManager.PlayerInteraction.ResetInteraction();
			GameManager.PlayerStatus.ResetStatus();

			FindObjectOfType<PlayerLookTarget>().SetToPlayerPosition();
			FindObjectOfType<SmoothObjectTracking>().SetToPlayerPosition();
			FindAllCheckpoints();
			InitialiseCheckpoints();
        }

		private void Update()
		{

        }


        /// <summary>
        /// Returns the currently active checkpoint
        /// </summary>
        /// <returns></returns>
        public Checkpoint GetActiveCheckpoint()
			{
				return m_levelCheckpoints[currentCheckpoint];
			}

            public void Reload()
            {
				Start();
            }
        }
	
}
