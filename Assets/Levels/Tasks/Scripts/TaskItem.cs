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
			[Tooltip("Events are called when the task is completed")]
			[SerializeField] private UnityEvent m_events;


			public void CompleteTask()
			{
				m_toggle.isOn = true;
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
				m_toggle.GetComponentInChildren<Text>().text = m_name;
				m_completedSubtasks = 0;
				if (m_subtasks)
					m_numberOfSubtasks = m_subtasks.childCount;
			}

			private void OnValidate()
			{
				m_toggle.GetComponentInChildren<Text>().text = m_name;
			}

			public void ActivateTask()
			{
				//Start at 30 increment by 30 for every subtask
				Vector2 screenStartSize = GetComponent<RectTransform>().sizeDelta;
				Vector2 screenEndSize = new Vector2(screenStartSize.x, 30 + (m_numberOfSubtasks * 30));

				Tween.Size(GetComponent<RectTransform>(), screenStartSize, screenEndSize, 0.5f, 0f);
				StartCoroutine(UpdateListGroup(screenEndSize));
				//LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);
			}

			public void DeactivateTask()
			{
				Vector2 screenStartSize = GetComponent<RectTransform>().sizeDelta;
				Vector2 screenEndSize = new Vector2(screenStartSize.x, 30);

				Tween.Size(GetComponent<RectTransform>(), screenStartSize, screenEndSize, 0.5f, 0f);
				StartCoroutine(UpdateListGroup(screenEndSize));
			}

			IEnumerator UpdateListGroup(Vector2 finalSize)
			{
				while (GetComponent<RectTransform>().sizeDelta.y < finalSize.y)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform.parent);
					yield return null;
				}
			}
		}
	}
}
