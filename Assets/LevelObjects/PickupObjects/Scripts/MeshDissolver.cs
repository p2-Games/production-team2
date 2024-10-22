///<summary>
/// Author: Halen
///
/// Makes objects melt like butter.
///
///</summary>

using Pixelplacement;
using System.Collections;
using System.Linq;
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
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
            Material material = new Material(mesh.material);

            // set the new material's shader to the dissolve shader
            material.shader = m_dissolveShader;

            // set the colour of the new material to the colour from the dissolve material
            material.SetColor("_DissolveColour", m_dissolveColour);
            material.SetFloat("_DissolveStrength", 0);

            // apply the material
            mesh.material = material;

            if (m_particle)
                Instantiate(m_particle);

            if (m_disableGravity)
                StartCoroutine(DisableGravity());
            Tween.ShaderFloat(material, "_DissolveStrength", 0.85f, m_duration, m_delay);
            Destroy(gameObject, m_delay + m_duration);
        }

        private IEnumerator DisableGravity()
        {
            yield return new WaitForSeconds(m_delay);

            GetComponent<Rigidbody>().useGravity = false;
        }
    }
}
