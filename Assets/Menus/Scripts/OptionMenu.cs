///<summary>
/// Author: Emily
///
/// Options Menu Handling
///
///</summary>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	namespace UI
	{
		public class OptionMenu : MonoBehaviour
		{
			[SerializeField] GameObject[] m_optionSubMenus;

			[Header("Sliders")]
			[SerializeField] private Slider m_masterVolSlider;
			[SerializeField] private Slider m_musicVolSlider;
			[SerializeField] private Slider m_sfxVolSlider;

			[SerializeField] private Slider m_verticalSensitivitySlider;
			[SerializeField] private Slider m_horizontalSensitivitySlider;

			[Header("Text Displays")]
			[SerializeField] private TextMeshProUGUI m_musicValueDisplay;
			[SerializeField] private TextMeshProUGUI m_sfxValueDisplay;

			[SerializeField] private TextMeshProUGUI m_verticalSensitivityValueDisplay;
			[SerializeField] private TextMeshProUGUI m_horizontalSensitivityValueDisplay;

            [Header("Sensitivity")]
            public float horizontalSensitivity;
            public float verticalSensitivity;

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
                m_musicVolSlider.value = PlayerSettings.Instance.musicVolume * 100;
                m_sfxVolSlider.value = PlayerSettings.Instance.sfxVolume * 100;

				float verticalMinMaxDifference = PlayerSettings.Instance.minMaxVerticalSpeed.y - PlayerSettings.Instance.minMaxVerticalSpeed.x;
				float horizontalMinMaxDifference = PlayerSettings.Instance.minMaxHorizontalSpeed.y - PlayerSettings.Instance.minMaxHorizontalSpeed.x;

				float vertVal = PlayerSettings.Instance.verticalSensitivity - PlayerSettings.Instance.minMaxVerticalSpeed.x;
				vertVal /= verticalMinMaxDifference; 

				m_verticalSensitivitySlider.value = vertVal * 100f;


				float horiVal = PlayerSettings.Instance.horizontalSensitivity - PlayerSettings.Instance.minMaxHorizontalSpeed.x;
				horiVal /= horizontalMinMaxDifference;

				m_horizontalSensitivitySlider.value = horiVal * 100f;
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
                m_verticalSensitivitySlider.onValueChanged.AddListener(delegate { AdjustVeritcalSensitivity(m_verticalSensitivitySlider.value); });
                m_horizontalSensitivitySlider.onValueChanged.AddListener(delegate { AdjustHorizontalSensitivity(m_horizontalSensitivitySlider.value); });
				m_musicVolSlider.onValueChanged.AddListener(delegate { AdjustMusicVolume(m_musicVolSlider.value); });
				m_sfxVolSlider.onValueChanged.AddListener(delegate { AdjustSFXVolume(m_sfxVolSlider.value); });

                gameObject.SetActive(false);
            }

			/// <summary>
			/// Sets the value of the vertical camera speed through PlayerSettings
			/// </summary>
			/// <param name="value"></param>
			public void AdjustVeritcalSensitivity(float value)
			{
				m_verticalSensitivityValueDisplay.text = value.ToString();
				value /= 100f;
				PlayerSettings.Instance.AdjustVerticalCameraSensitivity(value);
			}

            /// <summary>
            /// Sets the value of the horizontal camera speed through PlayerSettings
            /// </summary>
            /// <param name="value"></param>
            public void AdjustHorizontalSensitivity(float value)
			{
                m_horizontalSensitivityValueDisplay.text = value.ToString();
                value /= 100f;
                PlayerSettings.Instance.AdjustHorizontalCameraSensitivity(value);
            }

            /// <summary>
            /// Adjusts the output volume of music labeled sounds
            /// </summary>
            /// <param name="value"></param>
            public void AdjustMusicVolume(float value)
			{
				m_musicValueDisplay.text = value + "%";
                value /= 100f;
                PlayerSettings.Instance.AdjustMusicVolume(value);
			}

            /// <summary>
            /// Adjusts the output volume of sfx labeled sounds
            /// </summary>
            /// <param name="value"></param>
            public void AdjustSFXVolume(float value)
            {
                m_sfxValueDisplay.text = value + "%";
                value /= 100f;
                PlayerSettings.Instance.AdjustSFXVolume(value);
            }
        }
	}
}
