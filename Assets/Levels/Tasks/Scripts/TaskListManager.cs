///<summary>
/// Author: Emily
///
/// This will manage the task list and handle adding/removing objectives to the task list
///
///</summary>

using Millivolt.UI;
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


			private CanvasGroup m_canvasGroup;
			[SerializeField] private UIMenu m_menu;
			public UIMenu menu => m_menu;

			[SerializeField] private float m_appearDuration;
			public float appearDuration => m_appearDuration;

			private bool m_canActivate;
			public bool canActivate => m_canActivate;
			

			/// <summary>
			/// Hide all inactive tasks
			/// </summary>
			public void InitialiseTasks()
			{
				foreach (TaskItem task in inactiveTasks)
				{
					Tween.CanvasGroupAlpha(task.GetComponent<CanvasGroup>(), 0, 0, 0);
				}
			}

            private void Start()
            {
				m_menu.isActive = true;
				m_canActivate = true;
            }

            private void OnEnable()
            {
				InitialiseTasks();
				m_canvasGroup = GetComponent<CanvasGroup>();
            }

			/// <summary>
			/// Will set all task positions to the correct spots
			/// </summary>
            public void SpaceActiveTasks()
			{
				float space = 0;

				for (int i = 0; i < activeTasks.Count; i++)
				{

                    //I dont undertsnad why I have to add 80 on the end but whatEVER I GUESS
                    Vector2 taskPosition = new Vector2(0, ((space + m_spacing) * -1) + 80);

					taskPosition.y += m_spacing;

                    Tween.LocalPosition(activeTasks[i].transform, taskPosition, m_movingDuration, m_movingDelay, Tween.EaseInOut, Tween.LoopType.None, null, null, false);


					float subSpace = 0;

					for (int j = 0; j < activeTasks[i].activeTasks.Count; j++)
					{
						Vector2 subtaskPos = new Vector2(m_subtaskOffset, ((subSpace + m_spacing) * -1) + m_spacing);

						subtaskPos.y += m_spacing;

                        Tween.LocalPosition(activeTasks[i].activeTasks[j].transform, subtaskPos, m_movingDuration, m_movingDelay, Tween.EaseInOut, Tween.LoopType.None, null, null, false);
						subSpace += 30 + m_spacing;
					}

					space += activeTasks[i].taskSpace + m_spacing;
				}
			}

			/// <summary>
			/// Will toggle if the task list is visible on the screen or not
			/// </summary>
			public void SetTaskListActive(bool value)
			{
				if (value)
				{
					m_canActivate = false;
					m_menu.ActivateMenu();
					Invoke(nameof(ReenableAfterDelay), 0.5f);
				}
				else
				{
                       m_canActivate = false;
					m_menu.DeactivateMenu();
                       Invoke(nameof(ReenableAfterDelay), 0.5f);
                   }
			}

			private void ReenableAfterDelay()
			{
				m_canActivate = true;
			}
        }
	}
}
