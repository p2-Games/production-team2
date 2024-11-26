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

            [SerializeField] private GravityIndicatorUI m_gravityIndicatorUI;

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
                if (GameManager.Player && GameManager.Player.Controller.canMove)
                {
                    Vector3 movementDirection = GameManager.Player.Controller.movementDirection;
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

            /// <summary>
            /// Make the player face the specified position in world space.
            /// </summary>
            /// <param name="targetPosition"></param>
            public void SetHeading(Vector3 targetPosition)
            {
                Vector3 direction = targetPosition - transform.position;
                transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, transform.up).normalized, transform.up);
            }

            public void OnGravityChange()
            {
                // if the gravity indicator is not yet active, then activate it
                if (!m_gravityIndicatorUI.gameObject.activeSelf)
                    m_gravityIndicatorUI.gameObject.SetActive(true);

                m_gravityIndicatorUI.UpdateDirection();
                
                StartCoroutine(RotateWithGravity(-Physics.gravity.normalized));
            }

            private IEnumerator RotateWithGravity(Vector3 targetUp)
            {
                GameManager.Player.Controller.SetCanMove(CanMoveType.Gravity, false);

                //transform.rotation = transform.localRotation;
                Vector3 startUp = transform.up;
                float timer = 0;

                while (timer < m_gravityRotationTime)
                {
                    timer += Time.deltaTime;

                    float t = timer / m_gravityRotationTime;

                    t = -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;

                    transform.up = Vector3.Slerp(startUp, targetUp, t);

                    yield return null;
                }

                GameManager.Player.Controller.SetCanMove(CanMoveType.Gravity, true);
            }

            public void ResetRotation()
            {
                StopAllCoroutines();
                transform.rotation = Quaternion.identity;
            }

            public void ResetModel()
            {
                StopAllCoroutines();
                if (m_gravityIndicatorUI.gameObject.activeSelf)
                    m_gravityIndicatorUI.UpdateDirection();
            }
        }
    }
}
