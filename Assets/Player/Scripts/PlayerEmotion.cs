///<summary>
/// Author: Emily
///
/// Handles swapping the material on the face to display Robert's emotions
///
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt.Player
{
    public enum EmotionMode
    { 
        Default,
        Happy,
        Shocked,
        Sleepy
    }
    
    public class PlayerEmotion : MonoBehaviour
	{
        [System.Serializable]
        public class EmotionFace
        {
            [SerializeField] private EmotionMode m_type;
            [SerializeField] private Material m_regular;
            [SerializeField] private Material m_blink;

            public EmotionMode type => m_type;
            public Material regular => m_regular;
            public Material blink => m_blink;
        }

        [SerializeField] private GameObject m_head;
        
        [Tooltip("Input the index for the material that is the face")]
        [SerializeField] private int m_faceMatIndex;

        [Tooltip("Drag all the emotion materials in.\n" +
            "Make sure the order of emotions matches the same order shown in the dropdowns.")]
		[SerializeField] private List<EmotionFace> m_emotions;

        public string currentEmotion => nameof(m_currentEmotion.type);
        private EmotionFace m_currentEmotion;

        [Header("Blinking")]
        [SerializeField, Min(0)] private float m_blinkDuration;
        [SerializeField, Min(0)] private float m_blinkInterval;
        [SerializeField, Min(0)] private float m_blinkIntervalVariation;

        private float m_targetBlinkInterval;
        private float m_currentBlinkTime;

        private void Start()
        {
            StopAllCoroutines();

            ChangeEmotion(0);
        }

        public void Update()
        {
            // blinking logic
            m_currentBlinkTime += Time.deltaTime;

            if (m_currentBlinkTime >= m_targetBlinkInterval)
            {
                Blink();
                m_targetBlinkInterval = m_blinkInterval + Random.Range(-m_blinkIntervalVariation, m_blinkIntervalVariation);
                m_currentBlinkTime = 0;
            }
        }

        public void CycleEmotion(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                int emotionChoice = (int)m_currentEmotion.type + 1;
                if (emotionChoice == System.Enum.GetNames(typeof(EmotionMode)).Length)
                    emotionChoice = 0;
                ChangeEmotion((EmotionMode)emotionChoice);
            }
        }

        public void ChangeEmotion(string emotionName)
        {
            foreach (EmotionFace emotion in m_emotions)
            {
                if (nameof(emotion.type).Contains(emotionName))
                {
                    ChangeEmotion(emotion.type);
                    return;
                }
            }
        }

        public void ChangeEmotion(EmotionMode newEmotion)
        {
            if (m_currentEmotion != null && m_currentEmotion.type == newEmotion)
                return;

            // stop any blinking
            StopAllCoroutines();

            // set the current emotion
            m_currentEmotion = m_emotions[(int)newEmotion];

            // change the material to the base one by default
            ChangeMaterial(m_currentEmotion.regular);
        }

        public void Blink()
        {
            ChangeMaterial(m_currentEmotion.blink);
            StartCoroutine(ChangeMaterialOnDelay(m_currentEmotion.regular, m_blinkDuration));
        }

        /// <summary>
        /// ReInitalises all the materials on Robert. It HAS to be done like this (from my internet research) to properly get all the materials done correctly
        /// </summary>
        /// <param name="material"></param>
        private void ChangeMaterial(Material material)
        {
            //Temp array to hold all the materials on robert
            Material[] holdMats = m_head.GetComponent<SkinnedMeshRenderer>().materials;
            //Create instance of the material you want to change to
            Material emotionInstance = new(material);
            //Grabs the material in the array spot to change to the face you want
            holdMats[m_faceMatIndex] = emotionInstance;
            //Wipes the material list and replaces it with the new one
            m_head.GetComponent<SkinnedMeshRenderer>().materials = holdMats;
        }

        private IEnumerator ChangeMaterialOnDelay(Material material, float delay)
        {
            yield return new WaitForSeconds(delay);
            ChangeMaterial(material);
        }
    }
}
