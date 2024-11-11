///<summary>
/// Author: Halen
///
/// Logic for the loading screen.
///
///</summary>

using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	public class LoadingScreen : MonoBehaviour
	{
        [SerializeField] private Image m_loadingBar;

        private void OnEnable()
        {
            // m_loadingBar.fillAmount = 0;
        }

        public void UpdateLoad(float progress)
        {
            m_loadingBar.fillAmount = progress;
        }
    }
}
