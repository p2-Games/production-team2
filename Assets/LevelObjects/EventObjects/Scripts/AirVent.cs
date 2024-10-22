///<summary>
/// Author: Halen
///
/// Drops a static object and makes it
///
///</summary>

using UnityEngine;

namespace Millivolt.LevelObjects.EventObjects
{
    public class AirVent : EventObject
    {
        [Header("Air Vent Properties")]
        [SerializeField] GameObject[] m_objectsToDisable;
        [SerializeField] GameObject m_emissiveObject;
        
        private Rigidbody m_rb;
        private Material m_mat;

        private void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.useGravity = false;
            m_rb.isKinematic = true;

            m_mat = m_emissiveObject.GetComponent<MeshRenderer>().material;
            m_mat.SetColor("_EmissionColor", Color.red);
        }

        protected override void OnActivate()
        {
            m_rb.isKinematic = false;
            m_rb.useGravity = true;

            m_mat.SetColor("_EmissionColor", Color.green);

            foreach (GameObject obj in m_objectsToDisable)
            {
                obj.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!m_isActive)
                return;

            if (CanTrigger(collision.collider.gameObject))
            {
                GetComponent<MeshDissolver>().Dissolve();
            }
        }
    }
}
