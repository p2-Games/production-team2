///<summary>
/// Author: Halen
///
/// Drops a static object and makes it rotate and then it gets destroyed.
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
        [SerializeField, Min(0)] private float m_rotateSpeed = 2;
        [SerializeField, Min(0)] private float m_destroyDelay = 3;
        
        private Rigidbody m_rb;

        private void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_rb.useGravity = false;
            m_rb.isKinematic = true;
        }

        protected override void OnActivate()
        {
            StartCoroutine(RotateForward());
            //On activation set the objetc layer to ignore so that it wont collide with the player
            gameObject.layer = LayerMask.NameToLayer("Ignore");
            foreach (GameObject obj in m_objectsToDisable)
            {
                obj.SetActive(false);
            }
        }

        private IEnumerator RotateForward()
        {
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = initialRotation * Quaternion.Euler(transform.eulerAngles.z == 180 ? -25 : 25, 0, 0);

            float t = 0;
            while (transform.rotation != targetRotation)
            {
                yield return new WaitForFixedUpdate();
                t += Time.fixedDeltaTime * m_rotateSpeed;
                m_rb.MoveRotation(Quaternion.Lerp(initialRotation, targetRotation, t));
            }

            m_rb.isKinematic = false;
            m_rb.useGravity = true;

            Destroy(gameObject, m_destroyDelay);
        }
    }
}
