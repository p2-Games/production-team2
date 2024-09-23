///<summary>
/// Author: Emily
///
/// A base class for any menu in the game
/// Attach this script to any UI menu that is made to handle its interactions with the UI Manager
///
///</summary>

using UnityEditor;
using UnityEngine;
using Pixelplacement;

namespace Millivolt
{
	namespace UI
	{
		public class UIMenu : MonoBehaviour
		{
			private bool m_isActive;
			public bool isActive
			{
				get { return m_isActive; }
				set { m_isActive = value; }
			}

			[SerializeField] private bool m_hideOnInactive;
			public bool hideOnInactive => m_hideOnInactive;

            [SerializeField] private bool m_interactable = true;

            [Header("")]
            [SerializeField] private GameObject m_firstSelected;

            Vector2 m_screenScale
            {
                get { return GetComponent<RectTransform>().localScale; }
            }
            Rect m_screenSize 
            {
                get { return GetComponent<RectTransform>().rect; }
            }

            [Header("Tweening Properties")]
            [Space(20)]
            [SerializeField] private bool m_enableTweening;
            [Header("Activate tween info")]
            [Space(5)]
            [Header("Start Position")]
            [SerializeField, Range(-2, 2)] private float m_tweenEnableStartX;
            [SerializeField, Range(-2, 2)] private float m_tweenEnableStartY;
            //[Tooltip("The position that the menu will start at when it activates")]
            private Vector2 m_enableStartPos
            {
                get { return new Vector2((m_screenSize.width * m_tweenEnableStartX) * m_screenScale.x, (m_screenSize.height * m_tweenEnableStartY) * m_screenScale.y); }
            }
            [Tooltip("The position that the menu will end at when it activates")]
            [Header("End of activation Position")]
            [SerializeField, Range(-2, 2)] private float m_tweenEnableEndX;
            [SerializeField, Range(-2, 2)] private float m_tweenEnableEndY;
            private Vector2 m_enableEndPos
            {
                get { return new Vector2((m_screenSize.width * m_tweenEnableEndX) * m_screenScale.x, (m_screenSize.height * m_tweenEnableEndY) * m_screenScale.y); }
            }

            [Tooltip("How long the activation tween will last for")]
            [SerializeField] private float m_enableTweenDuration;
            [Tooltip("Delay before the tween activates")]
            [SerializeField] private float m_enableTweenDelay;
            [Space(20)]

            [Header("Deactivate tween info")]
            [Space(5)]
            [Header("Exit Position")]
            [SerializeField, Range(-2, 2)] private float m_tweenDisableEndX;
            [SerializeField, Range(-2, 2)] private float m_tweenDisableEndY;
            //[SerializeField] private Vector3 m_disableStartPos; 
            [Tooltip("The position that the menu will end at when it deactivates")]
            private Vector2 m_disableEndPos
            {
                get { return new Vector2((m_screenSize.width * m_tweenDisableEndX) * m_screenScale.x, (m_screenSize.height * m_tweenDisableEndY) * m_screenScale.y); }
            }
            [Tooltip("How long the deactivation tween will last for")]
            [SerializeField] private float m_disableTweenDuration;
            [Tooltip("Delay before the tween activates")]
            [SerializeField] private float m_disableTweenDelay;

            [SerializeField] private bool m_drawTweenGizmos;

            [Header("UI to tween")]
            [SerializeField] private GameObject m_uiGroup;

            /// <summary>
            /// Add the menu to the UI Managers activeMenu's list and play the activate tween
            /// </summary>
            public void ActivateMenu()
			{
                UIMenuManager.Instance.activeMenus.Insert(0, this);
                UIMenuManager.Instance.SetActiveMenu();
                UIMenuManager.Instance.CursorLockupdate();
                ActivateAnimation();
                if (m_interactable)
                {
                    EventSystemManager esm = FindObjectOfType<EventSystemManager>();
                    esm.SetCurrentSelectedGameObject(m_firstSelected);
                }
                m_isActive = true;
			}

			/// <summary>
			/// Remove the menu from the UI Managers activeMenu's list and play the deactivate tween
			/// </summary>
            public void DeactivateMenu()
            {
                UIMenuManager.Instance.activeMenus.Remove(this);
                UIMenuManager.Instance.SetActiveMenu();
                UIMenuManager.Instance.CursorLockupdate();
                DeactivateAnimation();
                m_isActive = false;
            }

            /// <summary>
            /// Play a tween for the menu activation
            /// </summary>
            private void ActivateAnimation()
            {
                if (!m_enableTweening)
                    gameObject.SetActive(true);
                else
                {
                    if (m_uiGroup != null)
                    {
                        gameObject.SetActive(true);
                        m_uiGroup.transform.position = m_enableStartPos;
                        Tween.Position(m_uiGroup.transform, m_enableEndPos, m_enableTweenDuration, m_enableTweenDelay, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
                    }
                }
            }

            /// <summary>
            /// Play a tween for the menu deactivation
            /// </summary>
            private void DeactivateAnimation()
            {
                if (!m_enableTweening)
                    gameObject.SetActive(false);
                else
                {
                    if (m_uiGroup != null)
                    {
                        Tween.Position(m_uiGroup.transform, m_disableEndPos, m_disableTweenDuration, m_disableTweenDelay, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
                        Invoke(nameof(HideMenuAfterTween), m_disableTweenDelay + m_disableTweenDuration);
                    }
                }
            }

            private void HideMenuAfterTween()
            {
                gameObject.SetActive(false);
            }

            private void Start()
            {
                
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (m_drawTweenGizmos)
                {
                    //print("Start pos" + m_enableStartPos);
                    //print("Screen size " + new Vector2(Screen.width, Screen.height));
                    Handles.color = Color.blue;
                    Handles.DrawWireDisc(m_enableStartPos, gameObject.transform.forward, 20);
                    Handles.DrawWireDisc(m_enableEndPos, gameObject.transform.forward, 20);
                    Handles.color = Color.red;
                    Handles.DrawLine(m_enableStartPos, m_enableEndPos);

                    Handles.color = Color.magenta;
                    //Handles.DrawWireDisc(m_enableStartPos, gameObject.transform.forward, 20);
                    Handles.DrawWireDisc(m_disableEndPos, gameObject.transform.forward, 20);
                    Handles.color = Color.red;
                    Handles.DrawLine(m_enableEndPos, m_disableEndPos);
                }
            }
#endif
        }
	}
}
