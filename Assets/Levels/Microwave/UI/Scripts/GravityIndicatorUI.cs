///<summary>
/// Author: Emily McDonald
///
/// Does the flashing for the indicator on the UI for gravity changing
///
///</summary>

using System.Collections;
using UnityEngine;

namespace Millivolt
{
    namespace UI
    {
        public class GravityIndicatorUI : MonoBehaviour
		{
            [SerializeField] private int m_flashCount;			
            
            [SerializeField] private GameObject m_gravText;
			[SerializeField] private GameObject[] m_indicators;

            public void StartFlash(float time)
            {
                StartCoroutine(GravityUIFlashing(time));
            }

            public IEnumerator GravityUIFlashing(float delay)
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

            private void UpdateDisplay(bool value)
            {
                m_gravText.SetActive(value);
                foreach (GameObject indicator in m_indicators)
                    indicator.SetActive(value);
            }
        }
	}
}
