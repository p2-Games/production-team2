///<summary>
/// Author: Emily
///
/// Class for handling the properties of a single task
///
///</summary>

using System.Collections.Generic;
using UnityEngine;

namespace Millivolt
{
	public class TaskItem : MonoBehaviour
	{
		public List<TaskItem> subtasks = new List<TaskItem>();

		[SerializeField] private string m_name;
		public string Name => m_name;

		private bool m_taskState;
		public bool taskState => m_taskState;
	}
}
