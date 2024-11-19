///<summary>
/// Author: Halen
///
/// Makes objects melt like butter.
///
///</summary>

using Pixelplacement;
using System.Collections;
using UnityEngine;

namespace Millivolt
{
    public class MeshDissolver : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private Color m_dissolveColour = new Color(0, 0, 0, 255);
        [SerializeField] private Shader m_dissolveShader;
        [SerializeField] private ParticleSystem m_particle;

        [Header("Dissolve Options")]
        [SerializeField, Min(0)] private float m_duration;
        [SerializeField, Min(0)] private float m_delay;
        [SerializeField] private bool m_disableGravity = true;

        public void Dissolve()
        {
            // get reference to current mesh, the colour set on the dissolve material,
            // and create a new material from the current mesh's material
            // also get the material for the outline/fill shader
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            Material[] materials = mesh.materials;
            Material material = new Material(materials[0]);
            Material outlineMaterial = new Material(materials[1]);

            // set the new material's shader to the dissolve shader
            material.shader = m_dissolveShader;

            // set the colour of the new material to the colour from the dissolve material
            material.SetColor("_DissolveColour", m_dissolveColour);
            material.SetFloat("_DissolveStrength", 0);

            // apply the material
            mesh.materials = new Material[] { material, outlineMaterial };

            // instantiate the particle effect if it exists
            if (m_particle)
                Instantiate(m_particle);

            // turn off gravity on the dissolve delay
            if (m_disableGravity)
                StartCoroutine(DisableGravity());

            // start the dissolve
            Tween.ShaderFloat(material, "_DissolveStrength", 0.85f, m_duration, m_delay);
            Tween.ShaderFloat(outlineMaterial, "_Scale", 0.85f, m_duration, m_delay);

            // destroy the object after the delay and total dissolve duration
            Destroy(gameObject, m_delay + m_duration);
        }

        private IEnumerator DisableGravity()
        {
            yield return new WaitForSeconds(m_delay);

            GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
