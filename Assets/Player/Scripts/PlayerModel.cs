///<summary>
/// Author: Halen
///
/// Handles the rotation of the player-model object, which is a child of the Player Controller.
///
///</summary>

using System.Collections;
using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerModel : MonoBehaviour
        {
            // collider
            private CapsuleCollider m_collider;
            new public CapsuleCollider collider => m_collider;
            
            [ContextMenu("Initialise Collider")]
            public void InitialiseCollider()
            {
                m_collider = GetComponent<CapsuleCollider>();

                PhysicMaterial pm = new PhysicMaterial();
                pm.frictionCombine = PhysicMaterialCombine.Minimum;
                pm.staticFriction = 0f;
                pm.dynamicFriction = 0f;
                pm.bounceCombine = PhysicMaterialCombine.Minimum;
                pm.bounciness = 0f;

                m_collider.material = pm;
            }

            [Header("Rotation/Heading")]
            [Tooltip("Degrees per second the player rotates towards its movement direction at.")]
            [SerializeField] private float m_forwardRotationSpeed = 60f;
            [Tooltip("Time it takes for the player to flip when the gravity changes.")]
            [SerializeField] private float m_gravityRotationTime = 0.5f;

            private float m_targetFoward;
            private float m_currentForward;

            private void Update()
            {
                // rotate player to face correct direction
                if (GameManager.PlayerController.canMove)
                {
                    Vector3 movementDirection = GameManager.PlayerController.movementDirection;
                    if (movementDirection != Vector3.zero)
                    {
                        m_targetFoward = Vector3.SignedAngle(transform.forward, movementDirection, transform.up);
                    }
                    m_currentForward = Mathf.MoveTowardsAngle(m_currentForward, m_targetFoward, m_forwardRotationSpeed * Time.deltaTime);
                    transform.rotation *= Quaternion.AngleAxis(m_currentForward, transform.up);
                }
            }

            public void OnGravityChange()
            {
                Vector3 gravityDir = Physics.gravity.normalized;
                Vector3 forward = new Vector3(gravityDir.z, gravityDir.x, gravityDir.y);
                StartCoroutine(RotateWithGravity(Quaternion.LookRotation(forward, -gravityDir)));
            }

            private IEnumerator RotateWithGravity(Quaternion targetAngle)
            {
                GameManager.PlayerController.canMove = false;

                //transform.rotation = transform.localRotation;
                Quaternion startAngle = transform.rotation;
                float timer = 0;

                while (timer < m_gravityRotationTime)
                {
                    timer += Time.deltaTime;

                    transform.rotation = Quaternion.Slerp(startAngle, targetAngle, timer / m_gravityRotationTime);
                    yield return new WaitForEndOfFrame();
                }

                GameManager.PlayerController.canMove = true;
            }
        }
    }
}