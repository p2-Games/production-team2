///<summary>
/// Author: Emily McDonald
///
/// This manages settings about the game
///
///</summary>

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Millivolt
{
	namespace Utilities
	{
		public enum GameState
		{
			MENU,
			PAUSE,
			PLAYING,
			FINISH
		}

		public class GameManager : MonoBehaviour
		{			
			private GameState m_gameState;

			[Header("Level Properties")]
			[SerializeField] private int m_currentLevel;

			[SerializeField] LevelData[] m_levels;

			public GameState gameState
			{
				get { return m_gameState; }
				set { m_gameState = value; }
			}

            private void Start()
            {
                
            }

            private void Update()
			{
				switch (m_gameState)
				{
					case GameState.MENU:
						break;
					case GameState.PAUSE:
						Time.timeScale = 0;
						break;
					case GameState.PLAYING:
						Time.timeScale = 1;
						break;
					case GameState.FINISH:
						break;
				}
			}

			/// <summary>
			/// Load the next level as set up in the current levels leveldata
			/// </summary>
			public void LoadNextLevel()
			{
				//SceneManager.LoadScene(m_levels[m_currentLevel].nextLevel.name);
			}

            /// <summary>
            /// Load the previous level as set up in the current levels leveldata
            /// </summary>
            public void LoadLastLevel()
			{
               // SceneManager.LoadScene(m_levels[m_currentLevel].prevLevel.name);
            }
		}
	}
}
