///<summary>
/// Author: Emily McDonald
///
/// This script will handle the level management
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace Utilities
	{
		public class LevelManager : MonoBehaviour
		{
            // level loading
			[SerializeField] private string m_prevLevelName;
            [SerializeField] private string m_nextLevelName;
			public string prevLevelName => m_prevLevelName;
			public string nextLevelName => m_nextLevelName;

			// checkpoints
			[SerializeField] public int currentCheckpoint;
			[SerializeField] private Checkpoint[] m_levelCheckpoints;
			[SerializeField] private bool m_autoAddCheckpoints;

			// level data
			[SerializeField] private LevelData m_levelData;
			public LevelData currentLevelData => m_levelData;

			// singleton pattern
			public static LevelManager Instance;

            private void Awake()
            {
				if (!Instance)
					Instance = this;
				else if (Instance != this)
					Destroy(gameObject);

				DontDestroyOnLoad(gameObject);
            }

            private void Start()
            {
				if (m_autoAddCheckpoints)
					FindAllCheckpoints();
				InitialiseCheckpoints();
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

			/// <summary>
			/// Returns the currently active checkpoint
			/// </summary>
			/// <returns></returns>
			public Checkpoint GetActiveCheckpoint()
			{
				return m_levelCheckpoints[currentCheckpoint];
			}
		}
	}
}
