///<summary>
/// Author: Emily
///
///
///
///</summary>

using Pixelplacement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
    namespace UI
    {
        public class ScreenFadeEffect : MonoBehaviour
        {
            [SerializeField, Min(0)] private float m_duration = 1;

            private CanvasGroup m_group;

            private void OnEnable()
            {
                m_group = GetComponent<CanvasGroup>();
                m_group.alpha = 1;
            }

            private void Update()
            {
                if (m_group.alpha > 0)
                {
                    float alpha = m_group.alpha;
                    alpha -= Time.deltaTime / m_duration;
                    if (alpha < 0)
                    {
                        alpha = 0;
                        gameObject.SetActive(false);
                    }

                    m_group.alpha = alpha;
                }
            }
        }
    }
}
