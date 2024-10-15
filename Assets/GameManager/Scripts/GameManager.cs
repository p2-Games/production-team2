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
    using UnityEditor;

	public enum GameState
	{
		MENU,
		PAUSE,
		PLAYING,
		FINISH
	}

	public class GameManager : MonoBehaviour
	{			
        public static GameManager Instance { get; private set; }

		// Player object static references
		public static PlayerController PlayerController { get; private set; }
		public static PlayerInteraction PlayerInteraction { get; private set; }
        public static PlayerStatus PlayerStatus { get; private set; }
		public static PlayerModel PlayerModel { get; private set; }

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
        }

        // Game state
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

        // level loading
        private string m_currentSceneName;
        private bool m_isLoading;

		public bool isLoading => m_isLoading;

        public void LevelSetup()
		{
			// get player references
			Transform player = FindObjectOfType<PlayerController>(true).transform;
			player.gameObject.SetActive(true);

			PlayerController = player.GetComponent<PlayerController>();
			PlayerInteraction = player.GetComponentInChildren<PlayerInteraction>();
			PlayerStatus = player.GetComponentInChildren<PlayerStatus>();
			PlayerModel = player.GetComponentInChildren<PlayerModel>();

            LevelManager.Instance.LevelSetup();
        }

		public void LoadScene(string sceneName)
		{
			m_currentSceneName = sceneName;
			UIMenuManager.Instance.ClearActiveMenus();
			SceneManager.LoadScene(sceneName);
		}

        public void LoadLevel(string levelName)
		{
			m_currentSceneName = levelName;
			UIMenuManager.Instance.ClearActiveMenus();
            StartCoroutine(LoadSceneAsync(levelName, LoadSceneMode.Single, false));
		}

        public void RestartLevel()
		{
			StartCoroutine(ReloadCurrentScene());
        }

		public void ExitToMenu()
		{
			gameState = GameState.MENU;
			SceneManager.LoadScene("MenuScene");
		}

		public void ExitGame()
		{
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public	IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool doSetup)
		{
			m_isLoading = true;
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

			while (!asyncLoad.isDone)
				yield return null;
			m_isLoading = false;

			if (doSetup)
				LevelSetup();
		}

		private IEnumerator UnloadSceneAsync(string sceneName)
		{
			UIMenuManager.Instance.ClearActiveMenus();

            m_isLoading = true;
			AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            while (!asyncUnload.isDone)
                yield return null;
			m_isLoading = false;
        }

		private IEnumerator ReloadCurrentScene()
		{
			StartCoroutine(UnloadSceneAsync(m_currentSceneName));
			while (!m_isLoading)
				yield return null;

			StartCoroutine(LoadSceneAsync(m_currentSceneName, LoadSceneMode.Additive, true));
        }

		// gravity methods
        public void ChangeGravity(Vector3 value)
        {
            Physics.gravity = value;
            PlayerModel.OnGravityChange();
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
    }

}
