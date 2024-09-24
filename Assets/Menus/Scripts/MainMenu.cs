///<summary>
/// Author: Emily
///
/// Starts the gameplay and exits the game
///
///</summary>

using Millivolt.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Millivolt
{
	public class MainMenu : MonoBehaviour
	{
        [SerializeField] private GameObject m_exitAnim;
        [SerializeField] private string m_gameSceneName;

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
