///<summary>
/// Author: Emily
///
/// Handles selecting a level and playing it
///
///</summary>

using Pixelplacement;
using System.Collections.Generic;
using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public class LevelSelect : MonoBehaviour
		{
			[SerializeField] private int m_selectedLevel;

			[SerializeField] private GameObject m_levelGroup;

			[SerializeField] private List<GameObject> m_levels;

			public void StartLevel()
			{

			}

			public void NextLevel()
			{
				m_selectedLevel++;
				if (m_selectedLevel > m_levels.Count - 1)
					m_selectedLevel = m_levels.Count - 1;
				UpdateLevelSelect();
			}

			public void PrevLevel()
			{
				m_selectedLevel--;
				if (m_selectedLevel < 0)
					m_selectedLevel = 0;
				UpdateLevelSelect();
			}

            private void UpdateLevelSelect()
            {
				Vector3 endPosition = new Vector3(m_selectedLevel * -300, 0, 0);

                Tween.LocalPosition(m_levelGroup.transform, endPosition, 0.5f, 0, Tween.EaseInOut);
            }
        }
	}
}
