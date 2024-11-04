///<summary>
/// Author: Halen
///
/// For controlling the animation of an object.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	public class AnimationController : MonoBehaviour
	{
		[SerializeField] private Animator m_animator;

		public void PlayAnimation(string name) => m_animator.Play(name);

		public void StopAnimation() => m_animator.StopPlayback();

		public void PassFloatParameter(string name, float value) => m_animator.SetFloat(name, value);
		public void PassIntParameter(string name, int value) => m_animator.SetInteger(name, value);
		public void PassBoolParameter(string name, bool value) => m_animator.SetBool(name, value);
		public void SetTriggerParameter(string name) => m_animator.SetTrigger(name);

		public float GetFloatParameter(string name) => m_animator.GetFloat(name);
		public int GetIntParameter(string name) => m_animator.GetInteger(name);
		public bool GetBoolParameter(string name) => m_animator.GetBool(name);
	}
}
