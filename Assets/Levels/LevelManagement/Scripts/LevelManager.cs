///<summary>
/// Author: Emily McDonald
///
/// This script will handle the level management
///
///</summary>

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Millivolt.UI;
using Millivolt.Player;

namespace Millivolt
{
	using Utilities;
		public class LevelManager : MonoBehaviour
		{
			public static LevelManager Instance { get; private set; }
            private void Awake()
            {
				if (!Instance)
					Instance = this;
				else if (Instance != this)
					Destroy(gameObject);

				DontDestroyOnLoad(gameObject);
            }
			
			[Header("Scene Properties")]
            [HideInInspector, SerializeField] private string m_prevLevelName;
            [HideInInspector, SerializeField] private string m_nextLevelName;

			public string prevLevelName => m_prevLevelName;
			public string nextLevelName => m_nextLevelName;

            //[Header("Checkpoint Properties")]
			[SerializeField] public int currentCheckpoint;
			[SerializeField] private Checkpoint[] m_levelCheckpoints;
			[Tooltip("This will find all the checkpoints in the scene and add them to the list automatically on play.\nWILL NOT SORT PROPERLY")]
			[SerializeField] private bool m_autoAddCheckpoints;

			[Header("Level Data")]
			[SerializeField] private LevelData m_levelData;

			[Header("Spawn references")]
			[SerializeField] private GameObject m_spawnScreen;

			public LevelData levelData => m_levelData;

			public PlayerController m_player;

            private void Start()
            {
				m_player = FindObjectOfType<PlayerController>();
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
				m_levelCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
				if (m_levelCheckpoints.Length == 0)
					Debug.Log("No checkpoints found in scene!");
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
				Instantiate(m_spawnScreen);
				m_player.transform.position = GetActiveCheckpoint().respawnPoint.position;
				m_player.transform.localEulerAngles = new Vector3(0, GetActiveCheckpoint().respawnPoint.localEulerAngles.y, 0);
				FindObjectOfType<PlayerLookTarget>().SetToPlayerPosition();
				FindObjectOfType<SmoothObjectTracking>().SetToPlayerPosition();
			}

		private void Update()
		{
			if (m_levelCheckpoints.Length == 0 || m_levelCheckpoints == null)
			{
				FindAllCheckpoints();
				InitialiseCheckpoints();
			}

			if (!m_player)
			{
                m_player = FindObjectOfType<PlayerController>(); 
				SpawnPlayer();
			}

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
