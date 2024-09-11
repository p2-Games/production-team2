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

		//[SerializeField] private LevelManager m_levelManager;
		[SerializeField] private EventSystemManager m_eventSystemManager;

		[SerializeField] private GameObject m_freeLookCam;

		[SerializeField] private UIMenu m_pauseMenu;

		private string m_currentSceneName;

		public static PlayerController PlayerController { get; private set; }
		public static PlayerInteraction PlayerInteraction { get; private set; }
        public static PlayerStatus PlayerStatus { get; private set; }

		public static LevelManager LevelManager { get; private set; }

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
			LevelManager = FindObjectOfType<LevelManager>();
			m_eventSystemManager = FindObjectOfType<EventSystemManager>();
			m_currentSceneName = SceneManager.GetActiveScene().name;

			// get player references
			GameObject player = GameObject.FindWithTag("Player");
			PlayerController = player.GetComponent<PlayerController>();
			PlayerInteraction = player.GetComponentInChildren<PlayerInteraction>();
			PlayerStatus = player.GetComponentInChildren<PlayerStatus>();

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
			SceneManager.LoadScene(LevelManager.nextLevelName);
		}

        /// <summary>
        /// Load the previous level as set up in the current levels leveldata
        /// </summary>
        public void LoadLastLevel()
		{
            SceneManager.LoadScene(LevelManager.prevLevelName);
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
			LevelManager.Reload();
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
