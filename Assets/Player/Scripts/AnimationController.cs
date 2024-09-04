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

		public void PassBoolParameter(string name, bool value) => m_animator.SetBool(name, value);
	}
}
