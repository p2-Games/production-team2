///<summary>
/// Author: Emily
///
/// This will manage the task list and handle adding/removing objectives to the task list
///
///</summary>

using Pixelplacement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	namespace Tasks
	{
		public class TaskListManager : MonoBehaviour
		{
			[SerializeField] private GameObject m_taskParent;



			[Space(30)]
			[Header("Task Layout")]
			[SerializeField] private float m_spacing;
			public float spacing => m_spacing;
			[SerializeField] private float m_subtaskOffset;
			public float subtaskOffset => m_subtaskOffset;
			[SerializeField] private float m_movingDuration;
			public float movingDuration => m_movingDuration;
			[SerializeField] private float m_movingDelay;
			public float moveDelay => m_movingDelay;

			[Space(30)]
			public List<TaskItem> activeTasks = new List<TaskItem>();
            public List<TaskItem> inactiveTasks = new List<TaskItem>();

			public void InitialiseTasks()
			{
				foreach (TaskItem task in inactiveTasks)
				{
					Tween.CanvasGroupAlpha(task.GetComponent<CanvasGroup>(), 0, 0, 0);
				}
			}
            private void OnEnable()
            {
				InitialiseTasks();
            }

            public void SpaceActiveTasks()
			{
				float space = 0;

				for (int i = 0; i < activeTasks.Count; i++)
				{

                    //I dont undertsnad why I have to add 80 on the end but whatEVER I GUESS
                    Vector2 taskPosition = new Vector2(0, (i * (space + m_spacing) * -1) + 80);

                    Tween.LocalPosition(activeTasks[i].transform, taskPosition, m_movingDuration, m_movingDelay, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
					space += activeTasks[i].taskSpace;
				}
			}
        }
	}
}
