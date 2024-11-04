///<summary>
/// Author: Halen
///
/// Drops a static object and makes it
///
///</summary>

using System.Collections;
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

            gameObject.layer = 10;

            StartCoroutine(RotateForward());

            foreach (GameObject obj in m_objectsToDisable)
            {
                obj.SetActive(false);
            }
        }

        private IEnumerator RotateForward()
        {
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = initialRotation * Quaternion.AngleAxis(25, transform.forward);

            float t = 0;
            while (transform.rotation != targetRotation)
            {
                yield return new WaitForFixedUpdate();
                t += Time.fixedDeltaTime;
                m_rb.MoveRotation(Quaternion.Lerp(initialRotation, targetRotation, t));
            }
        }
    }
}
