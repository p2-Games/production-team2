///<summary>
/// Author: Emily McDonald
///
/// Does the flashing for the indicator on the UI for gravity changing
///
///</summary>

using System.Collections;
using TMPro;
using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public class GravityIndicatorUI : MonoBehaviour
		{
			[SerializeField] private TextMeshProUGUI m_gravText;
			[SerializeField] private TextMeshProUGUI[] m_indicators;

            private void Start()
            {
                m_gravText.color = new Color(1, 0, 0, 0);
				foreach (TextMeshProUGUI text in m_indicators)
				{
					text.color = new Color(1, 0, 0, 0);
                }
            }

			public IEnumerator GravityUIFlashing(float intervalTime)
			{
				for (int i = 0; i < m_indicators.Length + 1; i++)
				{
                    yield return new WaitForSeconds(intervalTime);
                    for (int j = 0; j < i; j++)
					{
                        m_indicators[j].color = new Color(1, 0, 0, 1);
                    }
                    m_gravText.color = new Color(1, 0, 0, 1);
                    yield return new WaitForSeconds(intervalTime);
                    for (int j = 0; j < i; j++)
                    {
                        m_indicators[j].color = new Color(1, 0, 0, 0);
                    }
                    m_gravText.color = new Color(1, 0, 0, 0);                    
                }
			}
        }
	}
}
