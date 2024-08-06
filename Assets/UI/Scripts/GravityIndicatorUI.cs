///<summary>
/// Author: Emily McDonald
///
/// Does the flashing for the indicator on the UI for gravity changing
///
///</summary>

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

			public void GravityUIFlashing(float intervalTime)
			{

			}
        }
	}
}
