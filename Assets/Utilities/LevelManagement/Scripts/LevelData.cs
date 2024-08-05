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

            private void Start()
            {
				InitialiseCheckpoints();
            }

			private void InitialiseCheckpoints()
			{
				int id = 0;
				foreach (Checkpoint point in m_levelCheckpoints)
				{
					point.checkpointID = id;
					id++;
				}
			}

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

			public Checkpoint GetActiveCheckpoint()
			{
				return m_levelCheckpoints[currentCheckpoint];
			}
		}
	}
}
