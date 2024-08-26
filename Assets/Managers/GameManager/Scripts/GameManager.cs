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

			[SerializeField] LevelManager[] m_levels;

			public GameState gameState => m_gameState;

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
				SceneManager.LoadScene(m_levels[m_currentLevel].nextLevelName);
			}

            /// <summary>
            /// Load the previous level as set up in the current levels leveldata
            /// </summary>
            public void LoadLastLevel()
			{
               SceneManager.LoadScene(m_levels[m_currentLevel].prevLevelName);
            }

			public void PauseGame()
			{
				m_gameState = GameState.PAUSE;
			}

			public void UnpauseGame()
			{
				m_gameState = GameState.PLAYING;
			}
		}
	}
}
