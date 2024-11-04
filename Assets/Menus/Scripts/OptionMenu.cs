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

			/// <summary>
			/// Handles switching between the audio and camera settings
			/// </summary>
			/// <param name="value"></param>
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

			/// <summary>
			/// Calls the ActivateMenu function for the options menu and then sets the sliders to match the values of the sounds
			/// </summary>
			public void OpenOptions()
			{
				GetComponent<UIMenu>().ActivateMenu();
                m_musicVolSlider.value = PlayerSettings.Instance.musicVolume;
                m_sfxVolSlider.value = PlayerSettings.Instance.sfxVolume;
            }

			/// <summary>
			/// Calls the DeactivateMenu function for the options menu
			/// </summary>
			public void CloseOptions()
			{
				GetComponent<UIMenu>().DeactivateMenu();
			}

            private void Start()
            {
				// Set the slider values for the sounds to equal whatthey should be in settings
				m_musicVolSlider.value = PlayerSettings.Instance.musicVolume;
				m_sfxVolSlider.value = PlayerSettings.Instance.sfxVolume;

				// Add listeners to all the sliders and make changes call their correct settings
				m_sensitivitySlider.onValueChanged.AddListener(delegate { AdjustSensitivity(m_sensitivitySlider.value); });
				m_musicVolSlider.onValueChanged.AddListener(delegate { AdjustMusicVolume(m_musicVolSlider.value); });
				m_sfxVolSlider.onValueChanged.AddListener(delegate { AdjustSFXVolume(m_sfxVolSlider.value); });

                gameObject.SetActive(false);
            }

			/// <summary>
			/// Sets the value of the camera speed through PlayerSettings
			/// </summary>
			/// <param name="value"></param>
			public void AdjustSensitivity(float value)
			{
				PlayerSettings.Instance.AdjustCameraSensitivity(value);
			}

			/// <summary>
			/// Adjusts the output volume of music labeled sounds
			/// </summary>
			/// <param name="value"></param>
			public void AdjustMusicVolume(float value)
			{
				PlayerSettings.Instance.AdjustMusicVolume(value);
			}

            /// <summary>
            /// Adjusts the output volume of sfx labeled sounds
            /// </summary>
            /// <param name="value"></param>
            public void AdjustSFXVolume(float value)
            {
				PlayerSettings.Instance.AdjustSFXVolume(value);
            }
        }
	}
}
