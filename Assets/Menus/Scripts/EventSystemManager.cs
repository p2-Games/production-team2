///<summary>
/// Author: Halen
///
///
///
///</summary>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

            private void Start()
            {
                m_eventSystem = GetComponentInChildren<EventSystem>();
            }

            [Header("Component References")]
            [SerializeField] private EventSystem m_eventSystem;

            /// <summary>
            /// Set's the event system's currently selected game object for UI
            /// </summary>
            /// <param name="newSelectedObject"></param>
            public void SetCurrentSelectedGameObject(GameObject newSelectedObject)
            {
                m_eventSystem.SetSelectedGameObject(newSelectedObject);
                Button newSelectable = newSelectedObject.GetComponent<Button>();
                newSelectable.Select();
                newSelectable.OnSelect(null);
            }
        }
    }
}
