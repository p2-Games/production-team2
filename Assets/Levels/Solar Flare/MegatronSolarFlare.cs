///<summary>
/// Author: Halen
///
/// MonoBehavior that handles the charge up, explosion, and delays of the magnetron room.
///
///</summary>

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Millivolt.LevelObjects
{
	public class MegatronSolarFlare : LevelObject
	{
        public override bool isActive
		{
			get => base.isActive;
			set
			{
				m_isActive = value;
				StopAllCoroutines();
				if (value)
				{
					StartCoroutine(StartSolarFlarSequence());
                    m_chargeEffect.Play();
                }
				else
				{
					m_chargeEffect.Stop(false, ParticleSystemStopBehavior.StopEmitting);
				}
            }
		}

		[Header("Particle Effects")]
		[SerializeField] private ParticleSystem m_chargeEffect;
		[SerializeField] private ParticleSystem m_chargeSubEmitter;
		[SerializeField] private ParticleSystem m_flareBurst;

        [Header("Timing")]
		[SerializeField, Min(0)] private float m_startDelay;
		[SerializeField, Min(0)] private float m_chargeDuration;
		[SerializeField, Min(0)] private float m_endDelay;

		[Space(16)]
		// Events
		[SerializeField] private UnityEvent m_startEvents;
		[SerializeField] private UnityEvent m_chargeEvents;
		[SerializeField] private UnityEvent m_endEvents;

        private void Start()
        {
			InitParticles();
        }

        private IEnumerator StartSolarFlarSequence()
		{
			yield return new WaitForSeconds(m_startDelay);
			m_chargeEvents.Invoke();
			yield return new WaitForSeconds(m_chargeDuration);
			m_endEvents.Invoke();
			yield return new WaitForSeconds(m_endDelay);

			// loop
			StartCoroutine(StartSolarFlarSequence());
		}

		private void InitParticles()
		{
			float totalDuration = m_startDelay + m_chargeDuration + m_endDelay;

			// init charge effect
			var chargeMain = m_chargeEffect.main;
			chargeMain.duration = totalDuration;
			chargeMain.startLifetime = m_chargeDuration;

			var chargeEmission = m_chargeEffect.emission;
			chargeEmission.SetBurst(0, new ParticleSystem.Burst(m_startDelay, 1));

			// init charge sub emitter
			var chargeSubEmitterEmission = m_chargeSubEmitter.emission;
			int chargeEmissionCount = (int)chargeSubEmitterEmission.GetBurst(0).count.constant;
			chargeSubEmitterEmission.SetBurst(0, new ParticleSystem.Burst(0, 1, chargeEmissionCount, m_chargeDuration / chargeEmissionCount));

			// init flare burst particles
			var burstMain = m_flareBurst.main;
			burstMain.duration = totalDuration;

			var burstEmission = m_flareBurst.emission;
			burstEmission.SetBurst(0, new ParticleSystem.Burst(m_startDelay + m_chargeDuration, burstEmission.GetBurst(0).count));
		}
    }
}
