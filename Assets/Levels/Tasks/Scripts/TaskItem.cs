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

namespace Millivolt
{
	[Serializable]
	public class TaskItem : MonoBehaviour
	{
		//public List<TaskItem> subtasks = new List<TaskItem>();

		private int m_taskNum;

		[SerializeField] private string m_name;
		public string Name => m_name;

		private Toggle m_toggle;

		private TaskItem m_taskParent;

		public bool completed => m_toggle.isOn;

		[SerializeField] private UnityEvent m_events;

		public void CompleteTask()
		{
			m_toggle.isOn = true;
			m_events.Invoke();
			if (m_taskParent) { }
				//Call parent to check subtasks

		}

        //Check if subtask completed
		//Child calls this when task completed

        private void Start()
        {
            
        }
    }
}
