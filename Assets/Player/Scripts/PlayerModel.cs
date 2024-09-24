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
        [RequireComponent(typeof(CapsuleCollider))]
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
            [Tooltip("The angle in degrees between the player's current heading and target heading where the current heading will snap instead of Slerp")]
            [SerializeField] private float m_forwardSnapAngle = 5f;
            [Tooltip("Time it takes for the player to flip when the gravity changes.")]
            [SerializeField] private float m_gravityRotationTime = 0.5f;

            private void Update()
            {
                // rotate player to face correct direction
                if (GameManager.PlayerController.canMove)
                {
                    Vector3 movementDirection = GameManager.PlayerController.movementDirection;
                    // ensure the movement vector isn't zero
                    if (movementDirection != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(movementDirection, transform.up);

                        // if angle is within set amount, then snap
                        if (Quaternion.Angle(transform.rotation, targetRotation) < m_forwardSnapAngle)
                            transform.rotation = targetRotation;
                        // otherwise continue the slerp
                        else
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_forwardRotationSpeed * Time.deltaTime);
                    }
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

            public void ResetRotation()
            {
                StopAllCoroutines();
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
