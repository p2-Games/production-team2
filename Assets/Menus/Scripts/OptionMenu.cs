///<summary>
/// Author: Emily
///
/// Options Menu Handling
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

			public void OpenOptions()
			{
				GetComponent<UIMenu>().ActivateMenu();
			}

			public void CloseOptions()
			{
				GetComponent<UIMenu>().DeactivateMenu();
			}

            private void Start()
            {
				m_musicVolSlider.value = GameManager.PlayerSettings.musicVolume;
				m_sfxVolSlider.value = GameManager.PlayerSettings.sfxVolume;

				m_sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(m_sensitivitySlider.value); });
				m_musicVolSlider.onValueChanged.AddListener(delegate { AdjustMusicVolume(m_musicVolSlider.value); });
				m_sfxVolSlider.onValueChanged.AddListener(delegate { AdjustSFXVolume(m_sfxVolSlider.value); });

                gameObject.SetActive(false);
            }

			public void AdjustSensitivity(float value)
			{
				GameManager.PlayerSettings.AdjustCameraSensitivity(value);
			}

			public void AdjustMusicVolume(float value)
			{
				GameManager.PlayerSettings.AdjustMusicVolume(value);
			}

            public void AdjustSFXVolume(float value)
            {
				GameManager.PlayerSettings.AdjustSFXVolume(value);
            }
        }
	}
}
