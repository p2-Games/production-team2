///<summary>
/// Author: Halen
///
///
///
///</summary>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Millivolt
{
    namespace UI
    {
	    public class EventSystemManager : MonoBehaviour
	    {
            // Static reference
            public static EventSystemManager Instance { get; private set; }

            // Singleton instantiation
            private void Awake()
            {
                if (!Instance)
                    Instance = this;
                else if (Instance != this)
                    Destroy(gameObject);

                DontDestroyOnLoad(gameObject);
            }

            [Header("Component References")]
            public EventSystem eventSystem;

            /// <summary>
            /// Set's the event system's currently selected game object for UI
            /// </summary>
            /// <param name="newSelectedObject"></param>
            public void SetCurrentSelectedGameObject(GameObject newSelectedObject)
            {
                eventSystem.SetSelectedGameObject(newSelectedObject);
                Button newSelectable = newSelectedObject.GetComponent<Button>();
                newSelectable.Select();
                newSelectable.OnSelect(null);
            }

            private void Update()
            {
                if (!eventSystem)
                    eventSystem = FindObjectOfType<EventSystem>();
            }
            public void Reload()
            {

            }
        }
    }
}
