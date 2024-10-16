///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	namespace UI
	{
		public class OptionMenu : MonoBehaviour
		{
			[SerializeField] GameObject[] m_optionSubMenus;

			[SerializeField] Slider m_masterVolSlider;
			[SerializeField] Slider m_musicVolSlider;
			[SerializeField] Slider m_sfxVolSlider;
			
			[SerializeField] Slider m_sensitivitySlider;

			public void SwitchMenu(int value)
			{
				for (int i = 0; i < m_optionSubMenus.Length; i++)
				{
					if (i == value)
						m_optionSubMenus[i].SetActive(true);
					else
						m_optionSubMenus[i].SetActive(false);
				}
			}

            private void Start()
            {
				//m_sensitivitySlider.onValueChanged.AddListener(() => AdjustSensitivity());
            }

			public void AdjustSensitivity(float val)
			{
				
			}
        }
	}
}
