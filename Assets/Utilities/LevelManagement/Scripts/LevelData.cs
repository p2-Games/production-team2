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
			[SerializeField] private int m_currentCheckpoint;
			[SerializeField] private Checkpoint[] m_levelCheckpoints;

			public void SetActiveCheckpoint()
			{

			}
		}
	}
}
