///<summary>
/// Author: Emily
///
/// Starts the gameplay and exits the game
///
///</summary>

using Millivolt.UI;
using Pixelplacement;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Millivolt
{
	public class MainMenu : MonoBehaviour
	{
        [SerializeField] private GameObject m_exitAnim;
        [SerializeField] private string m_gameSceneName;

        [SerializeField] private GameObject m_menu;
        [SerializeField] private GameObject m_levelSelector;

        [SerializeField] private GameObject m_bg;

        [SerializeField] private int m_selectedButton;

        [SerializeField] private List<ButtonBehaviour> m_buttons;

        [SerializeField] private UIMenuManager m_menuManager;

        [Space(10)]

        [SerializeField] private Button m_menuStart;
        [SerializeField] private Button m_levelSelectStart;

        private void Start()
        {
            UIMenu menu = GetComponent<UIMenu>();
            menu.ActivateMenu();
            SetActiveButton(m_selectedButton);
        }

        public void SetActiveButton(int index)
        {
            m_selectedButton = index;
            for (int i = 0; i < m_buttons.Count; i++)
            {
                if (i == m_selectedButton)
                    m_buttons[i].ActivateButton();
                else
                    m_buttons[i].DeactivateButton();
            }
        }

        public void StartGame()
        {
            m_exitAnim.SetActive(true);
            Invoke(nameof(SwitchScene), 1f);
        }

        public void BackToMenu()
        {
            Tween.LocalPosition(m_menu.transform, Vector3.zero, 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_levelSelector.transform, new Vector3(1000, 0, 0), 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_bg.transform, new Vector3(30, 0, 0), 0.5f, 0, Tween.EaseInOut);
            m_menuStart.Select();
        }

        public void SelectLevel()
        {
            Tween.LocalPosition(m_menu.transform, new Vector3(-1000, 0, 0), 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_levelSelector.transform, Vector3.zero, 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_bg.transform, new Vector3(-30, 0 , 0), 0.5f, 0, Tween.EaseInOut);
            m_levelSelectStart.Select();
        }

        public void OptionsMenu()
        {

        }

        public void Credits()
        {

        }

        public void SwitchScene()
        {
            SceneManager.LoadScene(m_gameSceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}
