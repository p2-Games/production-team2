///<summary>
/// Author: Emily
///
///	Manages player specific Settings, so far just camera sensitivity
///
///</summary>

using Cinemachine;
using Millivolt.Cameras;
using UnityEngine;

namespace Millivolt
{
	public class PlayerSettings : MonoBehaviour
	{
        public static PlayerSettings Instance { get; private set; }

        [Header("Min/Max Sensitivity Speed")]
        [Tooltip("The min and max value of the verical speed")]
        [SerializeField] private Vector2 m_minMaxVerticalSpeed;
        public Vector2 minMaxVerticalSpeed => m_minMaxVerticalSpeed;
        [Tooltip("The min and max value of the horizontal speed")]
        [SerializeField] private Vector2 m_minMaxHorizontalSpeed;
        public Vector2 minMaxHorizontalSpeed => m_minMaxHorizontalSpeed;

        [Header("Sensitivity")]
        public float horizontalSensitivity = 0.25f;
        public float verticalSensitivity = 0.12f;

        private Vector2[] m_sensitivityVectors = new Vector2[2];

        private float m_musicVolume;
        public float musicVolume => m_musicVolume;

        private float m_sfxVolume;
        public float sfxVolume => m_sfxVolume;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            m_sensitivityVectors[0] = m_minMaxVerticalSpeed;
            m_sensitivityVectors[1] = m_minMaxHorizontalSpeed;

            m_musicVolume = 1;
            m_sfxVolume = 1;
        }

        public void AdjustVerticalCameraSensitivity(float value)
        {
            //Get Difference
            float minMaxDifference = m_sensitivityVectors[0].y - m_sensitivityVectors[0].x;

            //multiply slider value against the difference
            float sensitivityValue = minMaxDifference * value;

            //Add that value to the minimum and then apply to the camera
            float finalValue = m_sensitivityVectors[0].x + sensitivityValue;

            if (Camera.main.TryGetComponent(out CameraController cc))
                cc.UpdateSensitivity(horizontalSensitivity, finalValue);
            verticalSensitivity = finalValue;
        }

        public void AdjustHorizontalCameraSensitivity(float value)
        {
            //Get Difference
            float minMaxDifference = m_sensitivityVectors[1].y - m_sensitivityVectors[1].x;

            //multiply slider value against the difference
            float sensitivityValue = minMaxDifference * value;

            //Add that value to the minimum and then apply to the camera
            float finalValue = m_sensitivityVectors[1].x + sensitivityValue;

            if (Camera.main.TryGetComponent(out CameraController cc))
                cc.UpdateSensitivity(finalValue, verticalSensitivity);
            horizontalSensitivity = finalValue;
        }

        public void AdjustMusicVolume(float value)
        {
            m_musicVolume = value;
        }

        public void AdjustSFXVolume(float value)
        {
            m_sfxVolume = value;
        }
    }
}
