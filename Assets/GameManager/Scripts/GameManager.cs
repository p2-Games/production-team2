///<summary>
/// Author: Emily McDonald
///
/// This manages settings about the game and calls scenes to load
///
///</summary>

using Cinemachine;
using Millivolt.UI;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Millivolt
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
		[SerializeField] private GameState m_gameState;

		[Header("Level Properties")]
		[SerializeField] private int m_currentLevel;

		[SerializeField] private LevelManager[] m_levels;

		[SerializeField] private LevelManager m_levelManager;
		[SerializeField] private EventSystemManager m_eventSystemManager;

		[SerializeField] private GameObject m_freeLookCam;

		[SerializeField] private UIMenu m_pauseMenu;

		private string m_currentSceneName;

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

        private void Start()
        {
			m_freeLookCam = GameObject.FindWithTag("FreeLook");
			m_pauseMenu = (UIMenu)FindObjectOfType(typeof(UIMenu), true);
			m_levelManager = FindObjectOfType<LevelManager>();
			m_eventSystemManager = FindObjectOfType<EventSystemManager>();
			m_currentSceneName = SceneManager.GetActiveScene().name;
        }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
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
			if (gameState != GameState.PAUSE)
			{
				m_pauseMenu.ActivateMenu();
				m_freeLookCam.SetActive(false);
				gameState = GameState.PAUSE;
			}
			else
			{
				m_freeLookCam.SetActive(true);
				gameState = GameState.PLAYING;
				m_pauseMenu.DeactivateMenu();
			}
		}

        private void Update()
        {
            if (!m_freeLookCam)
                m_freeLookCam = GameObject.FindWithTag("FreeLook");

			if (!m_pauseMenu)
                m_pauseMenu = (UIMenu)FindObjectOfType(typeof(UIMenu), true);

        }
        public void RestartLevel()
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
			m_freeLookCam = null;
			m_pauseMenu = null;
			Start();
			m_levelManager.Reload();
			m_eventSystemManager.Reload();
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
