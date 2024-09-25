///<summary>
/// Author: Emily
///
///
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace UI
    {
        public class ApplianceSpin : MonoBehaviour
        {
            [SerializeField] private float m_rotationSpeed;

            private void Update()
            {
                transform.Rotate(new Vector3(0.2f, 1, 0.1f) * m_rotationSpeed * Time.deltaTime);
                //if (transform.rotation.y >= 360)
                //    transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
        }
    }
}
