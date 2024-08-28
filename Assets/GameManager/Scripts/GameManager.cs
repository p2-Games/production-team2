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

			[SerializeField] GameObject m_freeLookCam;

			//Static reference
			public static GameManager Instance { get; private set; }

			public GameState gameState
			{
				get => m_gameState;
				set
				{
                    switch (value)
                    {
                        case GameState.MENU:
							Time.timeScale = 1;
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
					m_gameState = value;
                }
			}

            private void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(this);
                }
                else
                {
                    Instance = this;
                }

                DontDestroyOnLoad(gameObject);
            }

            private void Start()
            {
                
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

			public void PauseGame(bool value)
			{
				if (value)
				{
					gameState = GameState.PAUSE;
					m_freeLookCam.SetActive(false);
				}
				else
				{
					gameState = GameState.PLAYING;
					m_freeLookCam.SetActive(true);
				}
			}

			public void RestartLevel()
			{
				SceneManager.LoadScene(m_levels[m_currentLevel].name);
            }

			public void ExitToMenu()
			{
				gameState = GameState.MENU;
				SceneManager.LoadScene("MenuScene");
			}

			public void ExitGame()
			{
				Application.Quit();
			}
		}
	}
}
