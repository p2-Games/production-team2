///<summary>
/// Author: Emily
///
/// Starts the gameplay and exits the game
///
///</summary>

using Millivolt.UI;
using Pixelplacement;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void Start()
        {
            UIMenu menu = GetComponent<UIMenu>();
            menu.ActivateMenu();
        }

        public void StartGame()
        {
            m_exitAnim.SetActive(true);
            Invoke(nameof(SwitchScene), 1f);
        }

        public void SelectLevel()
        {
            Tween.LocalPosition(m_menu.transform, new Vector3(-1000, 0, 0), 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_levelSelector.transform, Vector3.zero, 0.5f, 0, Tween.EaseInOut);
            Tween.LocalPosition(m_bg.transform, new Vector3(-30, 0 , 0), 0.5f, 0, Tween.EaseInOut);
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
        }
    }
}
