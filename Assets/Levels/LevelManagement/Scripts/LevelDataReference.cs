///<summary>
/// Author: Emily
///
/// Holds a reference to the scenes LevelData for the LevelManager to grab from
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	public class LevelDataReference : MonoBehaviour
	{
		[SerializeField] private LevelData m_levelData;
		public LevelData levelData => m_levelData;
	}
}
