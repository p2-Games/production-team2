///<summary>
/// Author: Emily
///
/// This will manage the task list and handle adding/removing objectives to the task list
///
///</summary>

using System.Collections.Generic;
using UnityEngine;

namespace Millivolt
{
	namespace Tasks
	{
		public class TaskListManager : MonoBehaviour
		{
			public List<TaskItem> taskList = new List<TaskItem>();

			private int m_activeTaskIndex;

			public void SetActiveTask(int index)
			{
				m_activeTaskIndex = index;
				for (int i = 0; i < taskList.Count; i++)
				{
					if (i == m_activeTaskIndex)
						taskList[m_activeTaskIndex].ActivateTask();
					else
						taskList[i].DeactivateTask();
				}
			}


			public void AddTask()
			{

			}

			public void RemoveTask()
			{

			}

			public void AddSubTask()
			{

			}

			public void RemoveSubTask()
			{

			}
		}
	}
}
