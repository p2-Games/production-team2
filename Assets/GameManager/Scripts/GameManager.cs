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
    using Millivolt.Cameras;
    using Millivolt.Tasks;

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

		public static PlayerComponents Player;

		private CameraController m_cameraController;

		[SerializeField] private GameObject m_loadingScreen;

		public static TaskListManager Tasklist;

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

        private void Start()
        {
			m_currentSceneName = SceneManager.GetActiveScene().name;
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
					case GameState.PLAYING:
						SetTimeScale(1);
						break;
                    case GameState.PAUSE:
                    case GameState.FINISH:
						SetTimeScale(0);
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

            m_cameraController.UpdateVcamSpeed();
        }

        // level loading
        private string m_currentSceneName;
		[HideInInspector] public bool isLoading = false;

        public void LevelSetup()
		{
			m_loadingScreen.SetActive(false);
			// destroy original Player references component
			if (Player)
				Destroy(Player);
			Player = gameObject.AddComponent<PlayerComponents>();

			if (Tasklist)
				Destroy(Tasklist);
			Tasklist = FindObjectOfType<TaskListManager>();

			m_cameraController = FindObjectOfType<CameraController>();

			SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_currentSceneName));
            LevelManager.Instance.LevelSetup();
        }

        public void LoadLevel(string levelName)
		{
			m_currentSceneName = levelName;
			UIMenuManager.Instance.ClearActiveMenus();
            isLoading = true;
			m_loadingScreen.SetActive(true);
            StartCoroutine(LoadSceneAsync(levelName, LoadSceneMode.Single));
		}

        public void RestartLevel()
		{
			m_loadingScreen.SetActive(true);

			LoadLevel(m_currentSceneName);
        }

		public void ExitToMenu()
		{
			gameState = GameState.MENU;
			LoadLevel("MenuScene");
		}

		public void ExitGame()
		{
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

			while (!asyncLoad.isDone)
				yield return null;
			isLoading = false;

			if (SceneManager.GetActiveScene().name == "MenuScene")
				m_loadingScreen.SetActive(false);
		}

		private IEnumerator UnloadSceneAsync(string sceneName)
		{
			UIMenuManager.Instance.ClearActiveMenus();
			AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

			while (!asyncUnload.isDone)
				yield return new WaitForFixedUpdate();
			isLoading = false;
		}

		// gravity methods
        public void ChangeGravity(Vector3 value)
        {
            Physics.gravity = value;
            Player.Model.OnGravityChange();
        }
        public void ChangeGravity(Vector3 eulerDirection, float magnitude)
        {
            ChangeGravity(Quaternion.Euler(eulerDirection) * Vector3.up * magnitude);
        }

		public void SetTimeScale(float value)
		{
			if (value < 0 || value > 1)
			{
				Debug.LogError("Cannot set time scale to a value less than 0 or greater than 1.");
				return;
			}
			Time.timeScale = value;
			m_cameraController?.UpdateVcamSpeed();
		}

		public void SetGravity(Vector3 value)
		{
			Physics.gravity = value;
			Player.Model.transform.up = -Physics.gravity.normalized;
		}
		public void SetGravity(Vector3 eulerDirection, float magnitude)
		{
			Physics.gravity = Quaternion.Euler(eulerDirection) * Vector3.up * magnitude;
			Player.Model.transform.up = -Physics.gravity.normalized;

        }
        [ContextMenu("Reset Gravity")]
        public void ResetGravity()
        {
            ChangeGravity(LevelManager.Instance.levelData.gravityDirection, LevelManager.Instance.levelData.gravityMagnitude);
        }
    }

}
