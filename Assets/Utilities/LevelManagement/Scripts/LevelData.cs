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
			[Tooltip("This will find all the checkpoints in the scene and add them to the list automatically on play.\nWILL NOT SORT PROPERLY")]
			[SerializeField] private bool m_autoAddCheckpoints;

			[Header("Level Gravity Settings")]
            [SerializeField] protected float m_gravity;
            public float gravity => m_gravity;
			[SerializeField] private GravityIndicatorUI m_gravityUI;

			[Header("Player Reference")]
			[SerializeField] private PlayerController m_player;

			/// <summary>
			/// Gravity is just rotation of the player so use this function to set the players rotation
			/// By Default gravity is set at 0
			/// </summary>
			/// <param name="newGravity"></param>
            public void ChangeGravity(Vector3 newGravity, float gravityChangeTime)
            {
                float indicatorFlashInterval = (gravityChangeTime / 6);
				m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(indicatorFlashInterval, newGravity));
            }

			public void SetPlayerUp(Vector3 value)
			{
				m_player.transform.up = value;
			}

            private void Start()
            {
				m_gravityUI.gameObject.SetActive(true);
				if (m_autoAddCheckpoints)
					FindAllCheckpoints();
				InitialiseCheckpoints();
            }

			private void FindAllCheckpoints()
			{
				m_levelCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
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
