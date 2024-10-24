///<summary>
/// Author: Emily
///
/// Handles the callable functions for the pause menu
///
///</summary>

using System.Collections.Generic;
using UnityEngine;

namespace Millivolt
{
    namespace UI
    {
        public class PauseMenu : MonoBehaviour
		{
            [SerializeField] private UIMenu m_menu;

            [SerializeField] private GameObject m_closeScreen;

            [SerializeField] private List<ButtonBehaviour> m_buttons;

            private int m_selectedOption;

            private void Awake()
            {
                SetActiveButton(m_selectedOption);
            }

            private void Start()
            {
                gameObject.SetActive(false);
            }

            public void SetActiveButton(int index)
            {
                m_selectedOption = index;
                for (int i = 0; i < m_buttons.Count; i++)
                {
                    if (i == m_selectedOption)
                        m_buttons[i].ActivateButton();
                    else
                        m_buttons[i].DeactivateButton();
                }
            }

            public void ResumeGame()
            {
                GameManager.Instance.PauseGame();
                m_menu.DeactivateMenu();
            }

            public void ResetToCheckpoint()
            {
                GameManager.Instance.PauseGame();
                GameManager.PlayerStatus.Die();
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
