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
        
        private Rigidbody m_rb;

        private void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.useGravity = false;
            m_rb.isKinematic = true;
        }

        protected override void OnActivate()
        {
            m_rb.isKinematic = false;
            m_rb.useGravity = true;

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
