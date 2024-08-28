///<summary>
/// Author:
///
///
///
///</summary>

using Millivolt.UI;
using Millivolt.Utilities;
using UnityEngine;

namespace Millivolt
{
	public class TESTINGTESTINGRAG : MonoBehaviour
	{
        [SerializeField] UIMenu m_pauseMenu;
        GameManager gameManager;
        private void Start()
        {
            Invoke("PauseYEAH", 2f);
        }

        void PauseYEAH()
        {
            m_pauseMenu.ActivateMenu();
        }
    }
}
