///<summary>
/// Author: Emily McDonald
///
/// This script will handle the level data and management
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
	namespace Utilities
	{
		public class LevelData : MonoBehaviour
		{
			[Header("Scene Properties")]
			[SerializeField] private SceneAsset m_prevLevel;
			[SerializeField] private SceneAsset m_nextLevel;

			public SceneAsset prevLevel
			{
				get { return m_prevLevel; }
				set { m_prevLevel = value; }
			}

			public SceneAsset nextLevel
			{
				get { return m_nextLevel; }
				set { m_nextLevel = value; }
			}

			[Header("Checkpoint Properties")]
			[SerializeField] public int currentCheckpoint;
			[SerializeField] private Checkpoint[] m_levelCheckpoints;

			[Header("Level Gravity Settings")]
            [SerializeField] protected Vector3 m_gravity = new Vector3(0, -9.81f, 0);
            public Vector3 gravity => m_gravity;
            public void ChangeGravity(Vector3 newGravity)
            {
                m_gravity = newGravity;
            }

            private void Start()
            {
				InitialiseCheckpoints();
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
