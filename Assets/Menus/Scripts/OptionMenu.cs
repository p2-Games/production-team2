///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public class OptionMenu : MonoBehaviour
		{
			[SerializeField] GameObject[] m_optionSubMenus;

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
		}
	}
}
