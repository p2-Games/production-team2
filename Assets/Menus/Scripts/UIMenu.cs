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

            [Header("")]
            [SerializeField] private GameObject m_firstSelected;

            [Header("Tweening Properties")]
            [SerializeField] private bool m_enableTweening;
            [Header("Activate tween info")]
            [Tooltip("The position that the menu will start at when it activates")]
            [SerializeField] private Vector2 m_enableStartPos;
            [Tooltip("The position that the menu will end at when it activates")]
            [SerializeField] private Vector2 m_enableEndPos;
            [Tooltip("How long the activation tween will last for")]
            [SerializeField] private float m_enableTweenDuration;
            [Tooltip("Delay before the tween activates")]
            [SerializeField] private float m_enableTweenDelay;
            [Header("Deactivate tween info")]
            //[SerializeField] private Vector3 m_disableStartPos; 
            [Tooltip("The position that the menu will end at when it deactivates")]
            [SerializeField] private Vector2 m_disableEndPos;
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
                ActivateAnimation();
                EventSystemManager esm = FindObjectOfType<EventSystemManager>();
                esm.SetCurrentSelectedGameObject(m_firstSelected);
                m_isActive = true;
			}

			/// <summary>
			/// Remove the menu from the UI Managers activeMenu's list and play the deactivate tween
			/// </summary>
            public void DeactivateMenu()
            {
                UIMenuManager.Instance.activeMenus.Remove(this);
                UIMenuManager.Instance.SetActiveMenu();
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

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (m_drawTweenGizmos)
                {
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
