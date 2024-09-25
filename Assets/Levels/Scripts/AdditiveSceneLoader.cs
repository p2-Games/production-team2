///<summary>
/// Author: Halen
///
/// Additively loads the specified scene into the current one.
///
///</summary>

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Millivolt
{
    namespace Utilities
    {
        public class AdditiveSceneLoader : MonoBehaviour
        {
            [SerializeField] private string m_sceneName;

            private void Awake()
            {
                if (m_sceneName == string.Empty)
                    return;

                // check that the scene is not already loaded by comparing against
                // all currently loaded scene names
                for (int s = 0; s < SceneManager.sceneCount; s++)
                {
                    // if the scene is already loaded, then quit
                    if (SceneManager.GetSceneAt(s).name == m_sceneName)
                        return;
                }

                // otherwise, load the scene
                StartCoroutine(GameManager.Instance.LoadSceneAsync(m_sceneName, LoadSceneMode.Additive, true));
            }
        }
    }
}
