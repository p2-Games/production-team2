///<summary>
/// Author: Halen
///
/// Additively loads the specified scene into the current one.
///
///</summary>

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Millivolt
{
    namespace Utilities
    {
        public class AdditiveSceneLoader : MonoBehaviour
        {
            [SerializeField] private string[] m_sceneNames;

            private Coroutine m_currentLoad;
            private void Start()
            {
                foreach (string name in m_sceneNames)
                {
                    if (name == string.Empty)
                        continue;

                    // check that the scene is not already loaded by comparing against
                    // all currently loaded scene names
                    bool isLoaded = false;
                    for (int s = 0; s < SceneManager.sceneCount; s++)
                    {
                        // if the scene is already loaded, then go to next scene to be loaded
                        if (SceneManager.GetSceneAt(s).name == name)
                        {
                            isLoaded = true;
                            break;
                        }
                    }
                    if (isLoaded)
                        continue;

                    // otherwise, load the scene
                    m_currentLoad = StartCoroutine(GameManager.Instance.LoadSceneAsync(name, LoadSceneMode.Additive, name == m_sceneNames.Last())); ;
                }
            }
        }
    }
}
