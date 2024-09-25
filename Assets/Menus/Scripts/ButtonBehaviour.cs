///<summary>
/// Author: Emily
///
///
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

            private void Start()
            {
                m_buttonObj = GetComponent<Button>();
            }

            public void ActivateButton()
            {
                m_selectIcon.SetActive(true);
                m_buttonObj.Select();
            }

            public void DeactivateButton()
            {
                m_selectIcon.SetActive(false);
            }
        }
    }
}
