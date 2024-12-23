///<summary>
/// Author: Emily
///
/// Script that handles the behaviour of the buttons
///
///</summary>

using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
    namespace UI
    {
        public class ButtonBehaviour : MonoBehaviour
        {
            [SerializeField] private GameObject m_selectIcon;
            [SerializeField] private Button m_buttonObj;

            public Button button => m_buttonObj;

            private void Start()
            {
                m_buttonObj = GetComponent<Button>();
            }

            public void ActivateButton()
            {
                m_buttonObj = GetComponent<Button>();
                if (m_selectIcon)
                    m_selectIcon.SetActive(true);
                m_buttonObj.Select();
            }

            public void DeactivateButton()
            {
                if (m_selectIcon)
                    m_selectIcon.SetActive(false);
            }
        }
    }
}
