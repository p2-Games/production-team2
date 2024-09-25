///<summary>
/// Author: Emily
///
/// Starts the gameplay and exits the game
///
///</summary>

using Millivolt.UI;
using Pixelplacement;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	public class MainMenu : MonoBehaviour
	{
        private int m_selectedButton;

        [Header("Menu")]
        [SerializeField] private ButtonBehaviour[] m_mainButtons;

        [SerializeField] private GameObject m_menu;
        [SerializeField] private GameObject m_bg;

        [Header("Level Select")]
        [SerializeField] private GameObject m_levelSelector;
        [SerializeField] private Button[] m_levelButtons;

        private void Start()
        {
            UIMenu menu = GetComponent<UIMenu>();
            menu.ActivateMenu();
            SetActiveButton(m_selectedButton);
        }

        public void SetActiveButton(int index)
        {
            m_selectedButton = index;
            for (int i = 0; i < m_mainButtons.Length; i++)
            {
                if (i == m_selectedButton)
                    m_mainButtons[i].ActivateButton();
                else
                    m_mainButtons[i].DeactivateButton();
            }
        }

        public void EnableMainMenu()
        {
            Tween.LocalPosition(m_menu.transform, Vector3.zero, 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_levelSelector.transform, new Vector3(1000, 0, 0), 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_bg.transform, new Vector3(30, 0, 0), 0.5f, 0, Tween.EaseInOut);
            m_mainButtons[0].button.Select();
        }

        public void EnableLevelSelect()
        {
            Tween.LocalPosition(m_menu.transform, new Vector3(-1000, 0, 0), 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_levelSelector.transform, Vector3.zero, 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_bg.transform, new Vector3(-30, 0 , 0), 0.5f, 0, Tween.EaseInOut);
            m_levelButtons[0].Select();
        }

        public void OptionsMenu()
        {

        }

        public void Credits()
        {

        }

        public void QuitGame()
        {
            GameManager.Instance.ExitGame();
        }
    }
}
