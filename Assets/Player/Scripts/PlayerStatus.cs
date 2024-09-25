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
    using Level;

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
            [Tooltip("This needs the screen effect material for the hurt effect")]
            //[SerializeField] private Material m_staticVignette;
            [SerializeField] private ScreenShaderController m_screenShaderController;

            [Header("Knockback Properties")]
            [SerializeField] private float m_horizontalForce;
            [SerializeField] private float m_verticalForce;
            public float knockbackForce => m_horizontalForce;

            private GameObject m_deathCanvas;

            [Header("Respawn Properties")]
            [SerializeField] private float m_respawnTime;
            //Player
            //private PlayerController m_player;

            private void Start()
            {
                m_currentHealth = m_maxHealth;
                //Theres no conversion to GameObject for some reason so I did a hold variable for now </3
                PlayerDeathUI hold = (PlayerDeathUI)FindObjectOfType(typeof(PlayerDeathUI), true);
                m_deathCanvas = hold.gameObject;
                m_screenShaderController = GetComponent<ScreenShaderController>();
                UpdateVignetteEffect();
            }

            private void OnDestroy()
            {
                m_currentHealth = m_maxHealth;
                UpdateVignetteEffect();
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


                UpdateVignetteEffect();

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
                ResetPlayer();

                // play a death sound effect
                SFXController.Instance.PlayRandomSoundClip("PlayerDamage", transform.parent);
            }

            public void ResetPlayer()
            {
                m_currentHealth = m_maxHealth;
                UpdateVignetteEffect();
                m_deathCanvas.SetActive(true);
                GameManager.PlayerController.canMove = false;
                LevelManager.Instance.Invoke(nameof(LevelManager.Instance.SpawnPlayer), m_respawnTime);
            }

            /// <summary>
            /// Places an impulse force on the player to knock them backwards after taking damage
            /// </summary>
            public void PlayerKnockback(HazardObject hazard)
            {
                //Find the closest point on the hazard collider to the player
                Vector3 closestPoint = hazard.gameObject.GetComponent<Collider>().ClosestPoint(GameManager.PlayerController.transform.position);
                
                //Get the direction that the player needs to be knocked towards
                Vector3 dir = (GameManager.PlayerController.transform.position - closestPoint).normalized;
                dir = Vector3.ProjectOnPlane(dir, GameManager.PlayerController.transform.up).normalized;

                //Launch the player backwards
                GameManager.PlayerController.SetExternalVelocity(dir * m_horizontalForce);

                //Launch player upwards based on vertical force
                GameManager.PlayerController.AddVerticalVelocity(m_verticalForce * -Physics.gravity.normalized);
            }

            public void ResetStatus()
            {
                m_currentHealth = m_maxHealth;
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
                    UpdateVignetteEffect();
                    yield return new WaitForSeconds(m_healthRegenRate);
                }
                if (m_currentHealth > m_maxHealth)
                    m_currentHealth = m_maxHealth;
            }

            /// <summary>
            /// Updates the static effect for how much damage you have taken
            /// </summary>
            private void UpdateVignetteEffect()
            {
                float effectAmount = 1 - (m_currentHealth / m_maxHealth);

                if (effectAmount > 0.1)
                {
                    //m_staticVignette.SetFloat("_VignetteIntensity", effectAmount);
                    m_screenShaderController.UpdateShader("_VignetteIntensity", effectAmount);
                }
                else
                    m_screenShaderController.UpdateShader("_VignetteIntensity", 0);
            }
        }
    }
}
