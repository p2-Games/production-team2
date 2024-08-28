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

            private void OnEnable()
            {
                m_gameManager.PauseGame(true);
            }

            public void ResumeGame()
            {
                m_gameManager.PauseGame(false);
                m_menu.DeactivateMenu();
            }

            public void RestartLevel()
            {
                //CALL RESTART LEVEL METHOD FROM THE LEVELMANAGER
            }

            public void ExitToMenu()
            {
                m_gameManager.ExitToMenu();
                Instantiate(m_closeScreen);
                Invoke("MenuSceneChange", 1f);
            }

            private void MenuSceneChange()
            {
                //MOVE SCENE TO MENU SCREEN
            }
        }
	}
}
