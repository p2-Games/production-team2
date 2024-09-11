///<summary>
/// Author: Emily
///
/// Controls a Blit Screen shader (This will hopefully stop those commit messages from appearing)
///
///</summary>

using Cyan;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Millivolt
{
	public class ScreenShaderController : MonoBehaviour
	{
		[SerializeField] private UniversalRendererData m_renderData = null;
		[SerializeField] private string m_shaderName = null;
		public string ShaderName => m_shaderName;

		[SerializeField] private string m_shaderFloat;
		public string ShaderFloat => m_shaderFloat;

		private Material m_holdMat;

		[SerializeField] private Material m_shaderMat;

		private bool TryGetFeature(out ScriptableRendererFeature feature)
		{
			feature = m_renderData.rendererFeatures.Where((f) => f.name == m_shaderName).FirstOrDefault();
			return feature != null;
		}

        private void Start()
        {
            m_holdMat = new Material(m_shaderMat);
			UpdateShaderValues(m_holdMat);
        }

        private void OnDestroy()
        {
            ResetShader();
        }

		public void UpdateShader(string floatName, float value)
		{
			if (TryGetFeature(out var feature))
			{
				m_shaderFloat = floatName;
                var blitScript = feature as Blit;

				blitScript.settings.blitMaterial = m_holdMat;
                var material = blitScript.settings.blitMaterial;


                material.SetFloat(floatName, value);
            }
		}

		private void UpdateShaderValues(Material material)
		{
			material.SetFloat("_FullScreenIntensity", 0.2f);
			material.SetFloat("_VignetteRadiusPower", 4f);
			material.SetFloat("_StaticNoiseAmount", 53.5f);
			material.SetFloat("_GlitchStrength", 65.9f);
			material.SetFloat("_FlickerStrength", 90f);
		}

        private void ResetShader()
		{
			if (TryGetFeature(out var feature))
			{
				feature.SetActive(true);
				m_renderData.SetDirty();

				var blitScript = feature as Blit;
				blitScript.settings.blitMaterial = m_shaderMat;
				var material = blitScript.settings.blitMaterial;

                material.SetFloat(m_shaderFloat, 0);
			}
		}
    }
}
