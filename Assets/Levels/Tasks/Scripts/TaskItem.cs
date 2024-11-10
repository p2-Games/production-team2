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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Millivolt
{
	namespace Tasks
	{

		[Serializable]
		public class TaskItem : MonoBehaviour
		{
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

			[SerializeField] private TaskListManager m_taskListManager;

			[SerializeField] private float m_points;
			private float m_currentPoints;

			[Space(30)]
			[SerializeField] private GameObject m_subtaskParent;

			[SerializeField] private float m_taskSpace;
			public float taskSpace => m_taskSpace;

            [Space(30)]
            public List<TaskItem> activeTasks = new List<TaskItem>();
            public List<TaskItem> inactiveTasks = new List<TaskItem>();


            //EVENTS
            [Tooltip("Events are called when the task is completed")]
			[SerializeField] private UnityEvent m_events;

			private Vector2 m_orignalPos
			{
				get { return GetComponent<RectTransform>().sizeDelta; }
			}

			[SerializeField] private CanvasGroup m_canvasGroup;


            private void OnEnable()
            {
				Start();
            }

            /// <summary>
            /// Updates the space a single task + substasks should take up
            /// </summary>
            private void UpdateTaskSpace()
			{
				m_taskSpace = 30 + (activeTasks.Count * 30) + (activeTasks.Count * m_taskListManager.spacing);
			}
            
            /// <summary>
            /// Makes sure that all the Inactive tasks are hidden
            /// </summary>
            public void InitiliseSubtask()
			{
                foreach (TaskItem task in inactiveTasks)
                {
                    Tween.CanvasGroupAlpha(task.GetComponent<CanvasGroup>(), 0, 0, 0);
                }
				m_subtaskParent.transform.position += new Vector3(m_taskListManager.subtaskOffset, 0, 0);

                UpdateTaskSpace();
            }

            private void Start()
			{
				m_taskParent = transform.parent.parent.parent.GetComponent<TaskItem>();
				m_taskListManager = GetComponentInParent<TaskListManager>();
                m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = m_name;
				m_currentPoints = 0;
				if (m_points > 1)
                    m_toggle.GetComponentInChildren<TextMeshProUGUI>().text += (" (" + m_currentPoints + "/" + m_points + ")");
				InitiliseSubtask();
				UpdateTaskSpace();
			}


            /// <summary>
            /// Updates Text of task in real time in editor
            /// </summary>
            private void OnValidate()
			{
				m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = m_name;
			}

            /// <summary>
            /// Sets a Main task as active
            /// </summary>
            /// <param name="delay"></param>
			public void ActivateTask(float delay)
			{
                if (delay > 0)
                {
                    StartCoroutine(DelayFunction(delay, ActivateTask));
                    return;
                }
                m_taskListManager.inactiveTasks.Remove(this);
				m_taskListManager.activeTasks.Add(this);
				Tween.CanvasGroupAlpha(m_canvasGroup, 1, 0.5f, 0, Tween.EaseIn);
				m_taskListManager.SpaceActiveTasks();
			}

            /// <summary>
            /// Sets a subtask as active
            /// </summary>
            /// <param name="delay"></param>
			public void ActivateSubtask(float delay)
			{
                if (delay > 0)
                {
                    StartCoroutine(DelayFunction(delay, ActivateSubtask));
                    return;
                }
                m_taskParent.inactiveTasks.Remove(this);
				m_taskParent.activeTasks.Add(this);
                Tween.CanvasGroupAlpha(m_canvasGroup, 1, 0.5f, 0, Tween.EaseIn);
				m_taskParent.UpdateTaskSpace();
				m_taskListManager.SpaceActiveTasks();
            }

            /// <summary>
            /// Will give a task a point, If the points match or exceed the number of points needed to complete the task then it will auto crossout the 
            /// task and set all subtasks inactive
            /// </summary>
			public void CompleteTask(float delay)
			{
                if (delay > 0)
                {
                    StartCoroutine(DelayFunction(delay, CompleteTask));
                    return;
                }
                m_currentPoints++;
				if (m_points > 1)
					m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = m_name + (" (" + m_currentPoints + "/" + m_points + ")");
                if (m_currentPoints >= m_points)
				{
                    foreach (TaskItem task in activeTasks)
					{
						m_events.AddListener(delegate () { task.DeactivateSubtask(0); });
                    }
					CrossoutTask();
                    //m_taskParent.UpdateTaskSpace();
                    //m_taskListManager.SpaceActiveTasks();
                }
			}

            /// <summary>
            /// Will give a subtask a point, If the points match or exceed the number of points needed to complete the task then it will auto crossout the subtask
            /// </summary>
            /// <param name="delay"></param>
            public void CompleteSubtask(float delay)
            {
                if (delay > 0)
                {
                    StartCoroutine(DelayFunction(delay, CompleteSubtask));
                    return;
                }
                m_currentPoints++;
				if (m_points > 1)
					m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = m_name + (" (" + m_currentPoints + "/" + m_points + ")");
                if (m_currentPoints >= m_points)
                    CrossoutSubtask();
            }

            /// <summary>
            /// Will turn the toggle on the task to ON, and put a strike through the words
            /// </summary>
            private void CrossoutTask()
			{
                m_toggle.isOn = true;
                m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = "<s>" + m_toggle.GetComponentInChildren<TextMeshProUGUI>().text + "</s>";
                m_events.Invoke();
            }

            /// <summary>
            /// Will turn the toggle on the subtask to ON, and put a strike through the words
            /// </summary>
            private void CrossoutSubtask()
            {
                m_toggle.isOn = true;
                m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = "<s>" + m_toggle.GetComponentInChildren<TextMeshProUGUI>().text + "</s>";
                m_events.Invoke();
            }

            /// <summary>
            /// Will fade out a Task and then resize the Tasklist
            /// </summary>
            /// <param name="delay"></param>
            public void DeactivateTask(float delay)
            {
				if (delay > 0)
				{
                    StartCoroutine(DelayFunction(delay, DeactivateTask));
                    return;
				}
                m_taskListManager.activeTasks.Remove(this);
                m_taskListManager.inactiveTasks.Add(this);
                Tween.CanvasGroupAlpha(m_canvasGroup, 0, 0.5f, 0, Tween.EaseIn);
                m_taskListManager.SpaceActiveTasks();
            }

            /// <summary>
            /// Will fade out a Subtask and then resize the Tasklist
            /// </summary>
            /// <param name="delay"></param>
            public void DeactivateSubtask(float delay)
            {
                if (delay > 0)
                {
                    StartCoroutine(DelayFunction(delay, DeactivateSubtask));
                    return;
                }
                m_taskParent.activeTasks.Remove(this);
                m_taskParent.inactiveTasks.Add(this);
                Tween.CanvasGroupAlpha(m_canvasGroup, 0, 0.5f, 0, Tween.EaseIn);
                m_taskParent.UpdateTaskSpace();
                m_taskListManager.SpaceActiveTasks();
            }		
            
            /// <summary>
            /// Will wait for a set amount of time and then call function
            /// </summary>
            /// <param name="delay"></param>
            /// <param name="function"></param>
            /// <returns></returns>
            private IEnumerator DelayFunction(float delay, Action<float> function)
            {
                yield return new WaitForSeconds(delay);
                function?.Invoke(0);
            }
		}
	}
}
