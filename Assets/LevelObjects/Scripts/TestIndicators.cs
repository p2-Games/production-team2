///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	public class TestIndicators : MonoBehaviour
	{
        bool m_dispenserActive = false;
        bool m_screwActive;
        public GameObject fakeScrew;
        [SerializeField] Animator m_animator;

        private void Start()
        {
            fakeScrew.SetActive(false);

        }

        //display visual indicators when triggering parts of the level
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "DispenserTest" && m_dispenserActive)
            {
                m_dispenserActive = false;
                fakeScrew.SetActive(true);
                m_screwActive = true;
            }
            else if (other.tag == "PressurePlateTest")
            {
                m_dispenserActive = true;
                other.gameObject.SetActive(false);
            }
            else if (other.tag == "SocketTest" && m_screwActive)
            {
                fakeScrew.SetActive(false);
                m_animator.Play("AccessDoorAMove");
                other.gameObject.SetActive(true);
            }

        }
    }
}
