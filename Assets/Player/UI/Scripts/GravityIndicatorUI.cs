///<summary>
/// Author: Emily McDonald and Halen
///
/// Does the flashing for the indicator on the UI for gravity changing
///
///</summary>

using System.Collections;
using TMPro;
using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        namespace UI
        {
            public class GravityIndicatorUI : MonoBehaviour
            {
                [SerializeField] private GameObject m_gravText;
                [SerializeField] private GameObject[] m_indicators;

                [Space, Tooltip("The number of times the indicators will flash at the player.")]
                [SerializeField] private int m_flashCount;

                public void StartGravityFlash(float delay)
                {
                    StartCoroutine(GravityUIFlash(delay));
                }

                private void UpdateDisplay(bool value)
                {
                    m_gravText.SetActive(value);

                    foreach (GameObject ui in m_indicators)
                        ui.SetActive(value);
                }

                private IEnumerator GravityUIFlash(float delay)
                {
                    float intervalTime = delay / m_flashCount;
                    
                    for (int f = 0; f < m_flashCount; f++)
                    {
                        UpdateDisplay(true);
                        yield return new WaitForSeconds(intervalTime / 2f);

                        UpdateDisplay(false);
                        yield return new WaitForSeconds(intervalTime / 2f);
                    }
                }
            }
        }
    }
}
