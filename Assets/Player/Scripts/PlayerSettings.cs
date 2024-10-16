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

        [Tooltip("The min and max value of the verical acceleration")]
        Vector2 m_verticalAcceleration;
        [Tooltip("The min and max value of the verical deceleration")]
        Vector2 m_verticalDeceleration;
        [Tooltip("The min and max value of the horizontal acceleration")]
        Vector2 m_horizontalAcceleration;
        [Tooltip("The min and max value of the horizontal deceleration")]
        Vector2 m_horizontalDeceleration;

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
            m_povSettings = m_camera.GetComponent<CinemachinePOV>();
        }

        //VALUES FOR CAMERA TO REMEMEBR
        //

        public void AdjustCameraSensitivity()
        {
            //Maths time

            float verticalAccVal = m_verticalAcceleration.y - m_verticalAcceleration.x;
        }
    }
}
