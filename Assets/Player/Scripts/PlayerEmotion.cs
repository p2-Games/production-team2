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
        public class Emotion
        {
            [SerializeField] private EmotionMode m_type;
            [SerializeField] private Color m_colour = new(0, 0, 0, 1);
            [SerializeField] private Material m_regularFace;
            [SerializeField] private Material m_blinkFace;
            [SerializeField] private bool m_canBlink = false;

            public EmotionMode type => m_type;
            public Color colour => m_colour;
            public Material regular => m_regularFace;
            public Material blink => m_blinkFace;
            public bool canBlink => m_canBlink;
        }

        [Header("Emission Change")]
        [SerializeField] private Material m_headMat;
        [SerializeField] private Material m_torsoMat;
        [SerializeField] private Material m_armsMat;
        [SerializeField] private Material m_legsMat;

        [Space]

        [SerializeField] private float m_colourTransitionDuration;

        [Header("Facial Expressions")]
        [SerializeField] private GameObject m_faceObject;

        [Tooltip("Input the index for the material that is the face")]
        [SerializeField] private int m_faceMatIndex;

        [Tooltip("Drag all the emotion materials in.\n" +
            "Make sure the order of emotions matches the same order shown in the dropdowns.")]
        [SerializeField] private List<Emotion> m_emotions;

        public string currentEmotion => m_currentEmotion.type.ToString();
        private Emotion m_currentEmotion;

        [SerializeField, Min(0)] private float m_blinkDuration;
        [SerializeField, Min(0)] private float m_blinkInterval;
        [SerializeField, Min(0)] private float m_blinkIntervalVariation;

        private float m_targetBlinkInterval;
        private float m_currentBlinkTime;

        private void Start()
        {
            StopAllCoroutines();

            m_currentEmotion = m_emotions[0];
            SetEmotion(0);

            m_targetBlinkInterval = m_blinkInterval + Random.Range(-m_blinkIntervalVariation, m_blinkIntervalVariation);
        }

        public void Update()
        {
            // blinking logic
            if (m_currentEmotion.canBlink)
            {
                m_currentBlinkTime += Time.deltaTime;

                if (m_currentBlinkTime >= m_targetBlinkInterval)
                {
                    Blink();
                    m_targetBlinkInterval = m_blinkInterval + Random.Range(-m_blinkIntervalVariation, m_blinkIntervalVariation);
                    m_currentBlinkTime = 0;
                }
            }
        }

        public void CycleEmotion(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.performed)
            {
                int emotionChoice = (int)m_currentEmotion.type + 1;
                if (emotionChoice == System.Enum.GetNames(typeof(EmotionMode)).Length)
                    emotionChoice = 0;
                ChangeEmotion((EmotionMode)emotionChoice);
            }
#endif
        }

        public void ChangeEmotion(string emotionName)
        {
            foreach (Emotion emotion in m_emotions)
            {
                if (emotion.type.ToString().Contains(emotionName))
                {
                    ChangeEmotion(emotion.type);
                    return;
                }
            }
        }

        public void ChangeEmotion(EmotionMode newEmotionMode)
        {
            if (m_currentEmotion?.type == newEmotionMode)
                return;

            // stop any blinking
            StopAllCoroutines();

            // get new emotion
            Emotion newEmotion = m_emotions[(int)newEmotionMode];
           
            // set the current emotion
            m_currentEmotion = newEmotion;
            m_currentBlinkTime = 0;

            // change the material to the base one by default
            ChangeFaceMaterial(newEmotion.regular);
            ChangeColour(newEmotion.colour);
        }

        public void Blink()
        {
            ChangeFaceMaterial(m_currentEmotion.blink);
            StartCoroutine(ChangeMaterialOnDelay(m_currentEmotion.regular, m_blinkDuration));
        }

        public void SetEmotion(EmotionMode newEmotionMode)
        {
            StopAllCoroutines();

            Emotion newEmotion = m_emotions[(int)newEmotionMode];
            m_currentEmotion = newEmotion;
            m_currentBlinkTime = 0;

            ChangeFaceMaterial(newEmotion.regular);
            SetColour(newEmotion.colour);
        }

        /// <summary>
        /// ReInitalises all the materials on Robert. It HAS to be done like this (from my internet research) to properly get all the materials done correctly
        /// </summary>
        /// <param name="material"></param>
        private void ChangeFaceMaterial(Material material)
        {
            //Temp array to hold all the materials on robert
            Material[] holdMats = m_faceObject.GetComponent<SkinnedMeshRenderer>().materials;
            //Create instance of the material you want to change to
            Material emotionInstance = new(material);
            //Grabs the material in the array spot to change to the face you want
            holdMats[m_faceMatIndex] = emotionInstance;
            //Wipes the material list and replaces it with the new one
            m_faceObject.GetComponent<SkinnedMeshRenderer>().materials = holdMats;
        }

        private void ChangeColour(Color colour)
        {
            StartCoroutine(ChangeColorOverTime(m_headMat, m_headMat.GetColor("_EmissionColor"), colour));
            StartCoroutine(ChangeColorOverTime(m_torsoMat, m_torsoMat.GetColor("_EmissionColor"), colour));
            StartCoroutine(ChangeColorOverTime(m_armsMat, m_armsMat.GetColor("_EmissionColor"), colour));
            StartCoroutine(ChangeColorOverTime(m_legsMat, m_legsMat.GetColor("_EmissionColor"), colour));
        }

        private void SetColour(Color colour)
        {
            m_headMat.SetColor("_EmissionColor", colour);
            m_torsoMat.SetColor("_EmissionColor", colour);
            m_armsMat.SetColor("_EmissionColor", colour);
            m_legsMat.SetColor("_EmissionColor", colour);
        }

        [ContextMenu("Reset Emotion")]
        public void ResetEmotion()
        {
            SetColour(m_emotions[0].colour);
        }

        private IEnumerator ChangeMaterialOnDelay(Material material, float delay)
        {
            yield return new WaitForSeconds(delay);
            ChangeFaceMaterial(material);
        }

        private IEnumerator ChangeColorOverTime(Material material, Color startColour, Color endColour)
        {
            float timer = 0;

            Vector3 start = new(startColour.r, startColour.g, startColour.b);
            Vector3 end = new(endColour.r, endColour.g, endColour.b);

            while (timer < m_colourTransitionDuration)
            {
                timer += Time.deltaTime;

                float t = timer / m_colourTransitionDuration;

                t = -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;

                // get vector value
                Vector3 currentColour = Vector3.Slerp(start, end, t);

                // set colour from vector values (alpha is always 1)
                material.SetColor("_EmissionColor", new(currentColour.x, currentColour.y, currentColour.z, 1)); 

                yield return null;
            }
        }

        public void SetRandomEmotion()
        {
            EmotionMode randomEmotion = (EmotionMode) Random.Range(0, System.Enum.GetNames(typeof(EmotionMode)).Length);

            SetEmotion(randomEmotion);
        }

        private void OnDisable()
        {
            SetEmotion(EmotionMode.Default);
        }
    }
}
