///<summary>
/// Author: Emily McDonald
///
/// This script will handle the level data and management
///
///</summary>

using UnityEditor;
using UnityEngine;
using Millivolt.UI;
using Millivolt.Player;

namespace Millivolt
{
	namespace Utilities
	{
		public class LevelData : MonoBehaviour
		{
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

			[Header("Level Settings")]
            [SerializeField] protected Vector3 m_defaultGravity;
            public Vector3 defaultGravity => m_defaultGravity;

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
