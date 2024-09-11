///<summary>
/// Author: Emily
///
/// Handles the callable functions for the pause menu
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace UI
    {
        public class PauseMenu : MonoBehaviour
		{
            private UIMenu m_menu;

            [SerializeField] private GameObject m_closeScreen;

            private void Awake()
            {
                m_menu = gameObject.GetComponent<UIMenu>();
            }

            public void ResumeGame()
            {
                GameManager.Instance.PauseGame();
                m_menu.DeactivateMenu();
            }

            public void RestartLevel()
            {
                GameManager.Instance.PauseGame();
                m_closeScreen.SetActive(true);
                Invoke(nameof(CallRestart), 1f);
            }

            public void ExitToMenu()
            {
                GameManager.Instance.PauseGame();
                m_closeScreen.SetActive(true);
                Invoke(nameof(CallExitToMenu), 1f);
            }

            private void CallRestart()
            {
                GameManager.Instance.RestartLevel();
            }

            private void CallExitToMenu()
            {
                GameManager.Instance.ExitToMenu();
            }
        }
	}
}
