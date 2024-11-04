///<summary>
/// Author: Emily
///
/// Control the colour changing of the emissions on the player model
///
///</summary>

using Pixelplacement;
using UnityEngine;

namespace Millivolt
{
	public class PlayerEmissionController : MonoBehaviour
	{


		[Header("Emotion Colours")]
		[SerializeField] private Color m_defaultColour;
		[SerializeField] private Color m_happyColour;
		[SerializeField] private Color m_shockedColour;

		[Header("PlayerMaterials")]
		[SerializeField] private Material m_headMat;
		[SerializeField] private Material m_torsoMat;
		[SerializeField] private Material m_armsMat;
		[SerializeField] private Material m_legsMat;

		[Header("Transition details")]
		[SerializeField] private float m_transitionDuration;
		[SerializeField] private float m_transitionDelay;

		public void SetEmissionDefault()
		{
			Tween.ShaderColor(m_headMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
			Tween.ShaderColor(m_torsoMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
			Tween.ShaderColor(m_armsMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
			Tween.ShaderColor(m_legsMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
		}

        public void SetEmissionHappy()
        {
            Tween.ShaderColor(m_headMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
            Tween.ShaderColor(m_torsoMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
            Tween.ShaderColor(m_armsMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
            Tween.ShaderColor(m_legsMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
        }

        public void SetEmissionShocked()
        {
            Tween.ShaderColor(m_headMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
            Tween.ShaderColor(m_torsoMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
            Tween.ShaderColor(m_armsMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
            Tween.ShaderColor(m_legsMat, "_EmissionColor", m_defaultColour, m_transitionDuration, m_transitionDelay, Tween.EaseIn);
        }
    }
}
