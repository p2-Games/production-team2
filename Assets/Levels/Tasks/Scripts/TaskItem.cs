///<summary>
/// Author: Emily
///
/// Class for handling the properties of a single task
///
///</summary>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Pixelplacement;

namespace Millivolt
{
	[Serializable]
	public class TaskItem : MonoBehaviour
	{
		//public List<TaskItem> subtasks = new List<TaskItem>();

		private int m_completedSubtasks;
		private int m_numberOfSubtasks;

		[SerializeField] private string m_name;
		public string Name => m_name;

		[SerializeField] private Toggle m_toggle;

		[SerializeField] private TaskItem m_taskParent;

		public bool completed => m_toggle.isOn;

		[SerializeField] Transform m_subtasks;

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
			//m_toggle = GetComponentInChildren<Toggle>();
			m_taskParent = transform.parent.parent.parent.GetComponent<TaskItem>();
			m_completedSubtasks = 0;
			if (m_subtasks)
				m_numberOfSubtasks = m_subtasks.childCount;
        }
    }
}
