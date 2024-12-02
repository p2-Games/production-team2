///<summary>
/// Author: Halen, Emily
///
/// Tracks the status of the player.
/// Calls functions related to the player, such as UI updates and call respawn for death
///
///</summary>

using System.Collections;
using UnityEngine;

namespace Millivolt
{
    using LevelObjects.HazardObjects;
    using Sound;

    namespace Player
    {
        using UI;

        public class PlayerStatus : MonoBehaviour
        {
            [SerializeField] private float m_maxHealth;
            public float maxHealth => m_maxHealth;
            private float m_currentHealth;

            //[Header("LevelData Reference")]
            //LevelManager m_levelManager;

            [Header("Health Regen Properties")]
            [Tooltip("This will be the number of seconds after taking damage that the player will begine to regen health")]
            [SerializeField] private float m_healthRegenTime;
            [Tooltip("How often health will be given back to the player")]
            [SerializeField] private float m_healthRegenRate;
            [Tooltip("The amount of health that will be restored by the regen rate")]
            [SerializeField] private float m_healthRegenAmount;
            private Coroutine m_regen;

            [Header("Hurt and Death effect references")]
            [SerializeField] private PlayerDeathUI m_deathScreenEffect;

            [Header("Knockback Properties")]
            [SerializeField] private float m_horizontalForce;
            [SerializeField] private float m_verticalForce;
            public float knockbackForce => m_horizontalForce;

            private void Start()
            {
                m_currentHealth = m_maxHealth;
            }

            private void OnDestroy()
            {
                m_currentHealth = m_maxHealth;
            }

            /// <summary>
            /// Handles the player taking damage
            /// </summary>
            /// <param name="value"></param>
            public void TakeDamage(float value)
            {
                if (m_regen != null)
                    StopCoroutine(m_regen);

                m_currentHealth -= value;

                m_regen = StartCoroutine(RegenHealth());
                if (m_currentHealth <= 0)
                    Die();
                else
                    // play a damage sound effect
                    SFXController.Instance.PlayRandomSoundClip("PlayerDamage", transform.parent);
            }

            /// <summary>
            /// This will be called once the player's health has hit 0, this will load the player to their last checkpoint and give them full health
            /// </summary>
            public void Die()
            {
                // stop the player from moving
                GameManager.Player.Controller.SetCanMove(CanMoveType.Dead, false);

                // stop the player from respawning if they are already
                PlayerRespawn.Instance.StopAllCoroutines();

                // stop regen
                StopAllCoroutines();

                // reset player health
                m_currentHealth = m_maxHealth;

                // toggle canvases
                m_deathScreenEffect.gameObject.SetActive(true);

                // disable interaction
                GameManager.Player.Interaction.ResetInteraction();

                // play a death sound effect
                SFXController.Instance.PlayRandomSoundClip("PlayerDamage", transform.parent);
            }

            /// <summary>
            /// Places an impulse force on the player to knock them backwards after taking damage
            /// </summary>
            public void PlayerKnockback(HazardObject hazard)
            {
                //Find the closest point on the hazard collider to the player
                Vector3 closestPoint = hazard.gameObject.GetComponent<Collider>().ClosestPoint(GameManager.Player.Controller.transform.position);
                
                //Get the direction that the player needs to be knocked towards
                Vector3 dir = (GameManager.Player.Controller.transform.position - closestPoint).normalized;
                dir = Vector3.ProjectOnPlane(dir, GameManager.Player.Controller.transform.up).normalized;

                //Launch the player backwards
                GameManager.Player.Controller.SetExternalVelocity(dir * m_horizontalForce);

                //Launch player upwards based on vertical force
                GameManager.Player.Controller.AddVerticalVelocity(m_verticalForce * -Physics.gravity.normalized);
            }

            /// <summary>
            /// IEnumerator for gradually giving the player back health as a regen effect
            /// </summary>
            /// <returns></returns>
            IEnumerator RegenHealth()
            {
                yield return new WaitForSeconds(m_healthRegenTime);
                while(m_currentHealth < m_maxHealth)
                {
                    m_currentHealth += m_healthRegenAmount;
                    yield return new WaitForSeconds(m_healthRegenRate);
                }
                if (m_currentHealth > m_maxHealth)
                    m_currentHealth = m_maxHealth;
            }
            
        }
    }
}
