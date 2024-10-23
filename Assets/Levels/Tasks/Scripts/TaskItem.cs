///<summary>
/// Author: Emily
///
/// Class for handling the properties of a single task
///
///</summary>

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Pixelplacement;
using TMPro;
using System.Linq;

namespace Millivolt
{
	namespace Tasks
	{

		[Serializable]
		public class TaskItem : MonoBehaviour
		{
			//public List<TaskItem> subtasks = new List<TaskItem>();

			private int m_completedSubtasks;
			private int m_numberOfSubtasks;

			public string Name => m_name;
			public bool completed => m_toggle.isOn;

			[Header("UI Object References")]
			[Tooltip("Drag the child toggle for this task into this field")]
			[SerializeField] private Toggle m_toggle;
			[Tooltip("If this task has subtasks then drag the parent 'Subtask' object in here")]
			[SerializeField] Transform m_subtasks;

			[Header("Task Details")]
			[Tooltip("This is the text that will appear on the task list")]
			[SerializeField] private string m_name;
			[Tooltip("If this is a subtask assign its parent task here")]
			[SerializeField] private TaskItem m_taskParent;
			//[Tooltip("If you dont want having all subtasks being complete to autocomplete the task then tick this")]
			//[SerializeField] private bool m_subtasksDontComplete;

			//EVENTS
			[Tooltip("Events are called when the task is completed")]
			[SerializeField] private UnityEvent m_events;

			private Vector2 m_orignalPos
			{
				get { return GetComponent<RectTransform>().sizeDelta; }
			}

			[SerializeField] private CanvasGroup m_canvasGroup;

			public void CompleteTask()
			{
				m_toggle.isOn = true;
                m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = "<s>" + m_name + "</s>";
                m_events.Invoke();
				if (m_taskParent)
					m_taskParent.CompleteSubtask();
			}

			public void CompleteSubtask()
			{
				m_completedSubtasks++;
				if (m_completedSubtasks >= m_numberOfSubtasks)
					CompleteTask();
			}

			private void Start()
			{
				m_taskParent = transform.parent.parent.parent.GetComponent<TaskItem>();
                m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = m_name;
                m_completedSubtasks = 0;
				if (m_subtasks)
					m_numberOfSubtasks = m_subtasks.childCount;
			}

			private void OnValidate()
			{
				m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = m_name;
			}

			public void ActivateTask()
			{
				FadeInTask();

				//Start at 30 increment by 30 for every subtask
				Vector2 screenStartSize = GetComponent<RectTransform>().sizeDelta;
				Vector2 screenEndSize = new Vector2(screenStartSize.x, 30 + (m_numberOfSubtasks * 40));

				Tween.Size(GetComponent<RectTransform>(), screenStartSize, screenEndSize, 0.5f, 0f);
				StartCoroutine(UpdateListGroup(screenEndSize));
				//LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);
			}

			public void DeactivateTask()
			{
				FadeOutTask();

				Vector2 screenStartSize = GetComponent<RectTransform>().sizeDelta;
				//Vector2 screenEndSize = new Vector2(screenStartSize.x, 30);
				Vector2 screenEndSize = new Vector2(m_orignalPos.x, 30);

				Tween.Size(GetComponent<RectTransform>(), screenStartSize, screenEndSize, 0.5f, 0f);
				StartCoroutine(UpdateListGroup(screenEndSize));
			}

			public void FadeInTask()
			{
				m_canvasGroup.alpha = 0;
				gameObject.SetActive(true);
                Tween.CanvasGroupAlpha(m_canvasGroup, 1, 1.5f, 0, Tween.EaseOut);
                StartCoroutine(FadeCheck(1));
            }

			public void FadeOutTask()
			{
				Tween.CanvasGroupAlpha(m_canvasGroup, 0, 1.5f, 0, Tween.EaseOut);
				StartCoroutine(FadeCheck(0));
			}

			IEnumerator FadeCheck(float alphaTarget)
			{				
				while (m_canvasGroup.alpha != alphaTarget)
				{
					yield return null;
				}

				

				int activeCount = m_taskParent.transform.Cast<Transform>().Where(child => child.gameObject.activeSelf).Count();

                Vector2 screenStartSize = GetComponent<RectTransform>().sizeDelta;
                Vector2 screenEndSize = new Vector2(screenStartSize.x, 30 + (activeCount * 40));

                Tween.Size(GetComponent<RectTransform>(), screenStartSize, screenEndSize, 0.5f, 0f);
                StartCoroutine(UpdateListGroup(screenEndSize));

                if (m_canvasGroup.alpha == 0)
                    gameObject.SetActive(false);
            }

			IEnumerator UpdateListGroup(Vector2 finalSize)
			{
				while (GetComponent<RectTransform>().sizeDelta.y != finalSize.y)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);
					yield return null;
				}
			}
		}
	}
}
