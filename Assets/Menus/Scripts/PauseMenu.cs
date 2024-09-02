///<summary>
/// Author: Emily
///
///
///
///</summary>

using Millivolt.Utilities;
using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public class PauseMenu : MonoBehaviour
		{
            private GameManager m_gameManager;
            private UIMenu m_menu;
            private LevelManager m_levelManager;

            [SerializeField] private GameObject m_closeScreen;

            private void Awake()
            {
                m_gameManager = FindObjectOfType<GameManager>();
                m_menu = gameObject.GetComponent<UIMenu>();
                m_levelManager = FindObjectOfType<LevelManager>();
            }

            public void ResumeGame()
            {
                m_gameManager.PauseGame();
                m_menu.DeactivateMenu();
            }

            public void RestartLevel()
            {
                m_gameManager.PauseGame();
                m_closeScreen.SetActive(true);
                Invoke(nameof(CallRestart), 1f);
            }

            public void ExitToMenu()
            {
                m_gameManager.PauseGame();
                m_closeScreen.SetActive(true);
                Invoke(nameof(CallExitToMenu), 1f);
            }

            private void CallRestart()
            {
                m_gameManager.RestartLevel();
            }

            private void CallExitToMenu()
            {
                m_gameManager.ExitToMenu();
            }
        }
	}
}
