///<summary>
/// Author: Emily
///
///	Manages player specific Settings, so far just camera sensitivity
///
///</summary>

using Cinemachine;
using UnityEngine;

namespace Millivolt
{
	public class PlayerSettings : MonoBehaviour
	{
        public static PlayerSettings Instance { get; private set; }

        [SerializeField] private CinemachineVirtualCamera m_camera;
        private CinemachinePOV m_povSettings;

        [Tooltip("The min and max value of the verical speed")]
        [SerializeField] private Vector2 m_minMaxVerticalSpeed;
        [Tooltip("The min and max value of the horizontal speed")]
        [SerializeField] private Vector2 m_minMaxHorizontalSpeed;

        private Vector2[] m_sensitivityVectors = new Vector2[2];

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
            m_povSettings = m_camera.GetCinemachineComponent<CinemachinePOV>();

            m_sensitivityVectors[0] = m_minMaxVerticalSpeed;
            m_sensitivityVectors[1] = m_minMaxHorizontalSpeed;
        }

        public void AdjustCameraSensitivity(float value)
        {
            float[] holdValues = new float[4];

            for (int i = 0; i < m_sensitivityVectors.Length; i++)
            {
                //Get Difference
                float minMaxDifference = m_sensitivityVectors[i].y - m_sensitivityVectors[i].x;

                //multiply slider value against the difference
                float sensitivityValue = minMaxDifference * value;

                //Add that value to the minimum and then apply to the camera
                float finalValue = m_sensitivityVectors[i].x + sensitivityValue;

                holdValues[i] = finalValue;
            }

            m_povSettings.m_VerticalAxis.m_MaxSpeed = holdValues[0];
            m_povSettings.m_HorizontalAxis.m_MaxSpeed = holdValues[1];

        }
    }
}
