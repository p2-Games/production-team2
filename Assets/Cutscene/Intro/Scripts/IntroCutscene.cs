///<summary>
/// Author: Emily
///
/// Play the intro cutscene
///
///</summary>

using Millivolt.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Millivolt
{
	namespace Cutscene
	{
		public class IntroCutscene : MonoBehaviour
		{
            [SerializeField] private VideoPlayer m_vp;

            [SerializeField] private CanvasGroup m_screenFade;

            [SerializeField] string m_mainMenu;

            private void Start()
            {
                m_vp.Prepare();
                StartCoroutine(StartVideo());
            }

            IEnumerator StartVideo()
            {                
                yield return new WaitForSeconds(1f);
                
                for (int i = 0; i < 10; i++)
                {
                    m_screenFade.alpha -= .1f;
                    yield return new WaitForSeconds(.01f);
                }

                m_vp.Play();

                //yield return new WaitForSeconds((float)m_vp.length);
                yield return new WaitForSeconds(7.5f);

                for (int i = 0; i < 10; i++)
                {
                    m_screenFade.alpha += .1f;
                    yield return new WaitForSeconds(.01f);
                }

                SceneManager.LoadScene(m_mainMenu);
            }
        }
	}
}
