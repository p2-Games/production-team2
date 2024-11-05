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

		public static PlayerComponents Player;		

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
		public bool isLoading = false;

        public void LevelSetup()
		{
			// destroy original Player references component
			if (Player)
				Destroy(Player);
			Player = gameObject.AddComponent<PlayerComponents>();
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_currentSceneName));
            LevelManager.Instance.LevelSetup();
        }

        public void LoadLevel(string levelName)
		{
			m_currentSceneName = levelName;
			UIMenuManager.Instance.ClearActiveMenus();
            isLoading = true;
            StartCoroutine(LoadSceneAsync(levelName, LoadSceneMode.Single));
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

        public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

			while (!asyncLoad.isDone)
				yield return null;
			isLoading = false;
		}

		private IEnumerator UnloadSceneAsync(string sceneName)
		{
			UIMenuManager.Instance.ClearActiveMenus();
			AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            while (!asyncUnload.isDone)
                yield return new WaitForFixedUpdate();
			isLoading = false;
        }

		private IEnumerator ReloadCurrentScene()
		{
            // unload the game scene
			isLoading = true;
			StartCoroutine(UnloadSceneAsync(m_currentSceneName));
			while (isLoading)
				yield return new WaitForFixedUpdate();

			// reload the game scene
			isLoading = true;
            StartCoroutine(LoadSceneAsync(m_currentSceneName, LoadSceneMode.Additive));
			while (isLoading)
				yield return new WaitForFixedUpdate();

			// redo level setup
			LevelSetup();
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
