///<summary>
/// Author: Emily McDonald
///
/// This manages settings about the game and calls scenes to load
///
///</summary>

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Millivolt
{
    using Player;
    using UI;
	using Level;

	public enum GameState
	{
		MENU,
		PAUSE,
		PLAYING,
		FINISH
	}

	public class GameManager : MonoBehaviour
	{			
		[SerializeField] private GameState m_gameState;

		[SerializeField] private UIMenu m_pauseMenu;
		private UIMenu pauseMenu
		{
			get
			{
                if (!m_pauseMenu)
				{
					PauseMenu pm = (PauseMenu)FindObjectOfType(typeof(PauseMenu), true);
                    m_pauseMenu = pm.GetComponent<UIMenu>();
				}
				return m_pauseMenu;
            }
        }

		private string m_currentSceneName;

		public static PlayerController PlayerController { get; private set; }
		public static PlayerInteraction PlayerInteraction { get; private set; }
        public static PlayerStatus PlayerStatus { get; private set; }

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
			if (!Instance)
				Instance = this;
			else if (Instance != this)
			{
				Destroy(gameObject);
				return;
			}
            DontDestroyOnLoad(gameObject);

			Setup();
        }

		private void Setup()
		{
			m_currentSceneName = SceneManager.GetActiveScene().name;

			// get player references
			GameObject player = GameObject.FindWithTag("Player");
			PlayerController = player.GetComponent<PlayerController>();
			PlayerInteraction = player.GetComponentInChildren<PlayerInteraction>();
			PlayerStatus = player.GetComponentInChildren<PlayerStatus>();
		}

        /// <summary>
        /// Load the next level as set up in the current levels leveldata
        /// </summary>
        public void LoadNextLevel()
		{
			SceneManager.LoadScene(LevelManager.Instance.nextLevelName);
		}

        /// <summary>
        /// Load the previous level as set up in the current levels leveldata
        /// </summary>
        public void LoadLastLevel()
		{
            SceneManager.LoadScene(LevelManager.Instance.prevLevelName);
        }

		public void PauseGame()
		{
			if (gameState != GameState.PAUSE)
			{
				pauseMenu.ActivateMenu();
				gameState = GameState.PAUSE;
			}
			else
			{
				gameState = GameState.PLAYING;
				pauseMenu.DeactivateMenu();
			}
		}

		public void ChangeGravity(Vector3 value)
		{
			Physics.gravity = value;
			PlayerController.OnGravityChange();
		}
		public void ChangeGravity(Vector3 eulerDirection, float magnitude)
		{
			ChangeGravity(Quaternion.Euler(eulerDirection) * Vector3.up * magnitude);
		}
		[ContextMenu("Reset Gravity")]
		public void ResetGravity()
		{
			ChangeGravity(LevelManager.Instance.levelData.gravityDirection, LevelManager.Instance.levelData.gravityMagnitude);
		}
		
        public void RestartLevel()
		{
			StartCoroutine(LoadAsyncScene(SceneManager.GetActiveScene().name));
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

		public void Reload()
		{
			Setup();
			LevelManager.Instance.Reload();
			EventSystemManager.Instance.Reload();
		}

		IEnumerator LoadAsyncScene(string sceneName)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

			while (!asyncLoad.isDone)
			{
				yield return null;
			}

			Reload();
		}
	}
	
}
