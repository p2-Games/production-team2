///<summary>
/// Author: Halen
///
/// Additively loads the specified scene into the current one.
///
///</summary>

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Millivolt
{
    namespace Utilities
    {
        public class AdditiveSceneLoader : MonoBehaviour
        {
            [SerializeField] private string[] m_sceneNames;

            private void Start()
            {
                StartCoroutine(LoadScenes());
            }

            private IEnumerator LoadScenes()
            {
                foreach (string name in m_sceneNames)
                {
                    if (name == string.Empty)
                        continue;

                    // check that the scene is not already loaded by comparing against
                    // all currently loaded scene names
                    bool sceneIsAlreadyLoaded = false;
                    for (int s = 0; s < SceneManager.sceneCount; s++)
                    {
                        // if the scene is already loaded, then go to next scene to be loaded
                        if (SceneManager.GetSceneAt(s).name == name)
                        {
                            sceneIsAlreadyLoaded = true;
                            break;
                        }
                    }
                    if (sceneIsAlreadyLoaded)
                        continue;

                    // wait until game manager has finished loading a scene until the next one starts to load
                    while (GameManager.Instance.isLoading)
                        yield return new WaitForFixedUpdate();

                    // otherwise, load the scene
                    GameManager.Instance.isLoading = true;
                    StartCoroutine(GameManager.Instance.LoadSceneAsync(name, LoadSceneMode.Additive)); ;
                }

                StartCoroutine(CheckLoadingIsDone());
            }

            private IEnumerator CheckLoadingIsDone()
            {
                while (GameManager.Instance.isLoading)
                    yield return new WaitForFixedUpdate();

                GameManager.Instance.LevelSetup();
            }
        }
    }
}
