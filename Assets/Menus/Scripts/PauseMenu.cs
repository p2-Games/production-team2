///<summary>
/// Author: Emily
///
/// Handles the callable functions for the pause menu
///
///</summary>

using Millivolt.Level;
using System.Collections.Generic;
using TMPro;
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

            [Header("Timer references")]
            [SerializeField] private TextMeshProUGUI m_currentTimeText;

            private int m_selectedOption;

            private bool m_tasklistVisibilty;

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

            private void OnEnable()
            {
                // Calculate the time in minutes, seconds and milliseconds
                float timer = LevelManager.Instance.levelTimer;
                int seconds = (int)timer % 60;
                int milliseconds = (int)(timer % 1 * 100);
                int minutes = (int)timer / 60;

                // Format text into minutes and seconds 
                string timerText = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

                // Set the value of the timer text to whatever the value of the timer is in LevelManager
                m_currentTimeText.text = timerText;

                if (!GameManager.Tasklist)
                    return;

                m_tasklistVisibilty = GameManager.Tasklist.menu.isActive;
                if (!m_tasklistVisibilty)
                    GameManager.Tasklist.SetTaskListActive(true);
            }

            public void ResumeGame()
            {
                GameManager.Instance.PauseGame();
                m_menu.DeactivateMenu();                
            }

            public void ResetToCheckpoint()
            {
                GameManager.Instance.PauseGame();
                GameManager.Player.Status.Die();
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

            public void UpdateTaskListState()
            {
                if (!m_tasklistVisibilty)
                    GameManager.Tasklist.SetTaskListActive(false);
            }

        }
	}
}
