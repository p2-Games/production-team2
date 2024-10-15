///<summary>
/// Author: Halen
///
/// Plays a sound on start.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] string m_collectionName;
        [SerializeField] string m_clipName;

        [SerializeField] private bool m_playOnStart = false;
        [SerializeField] private bool m_attachToPlayer = false;
        
        private void Start()
        {
            if (m_playOnStart)
                PlaySound();
        }

        public void PlaySound()
        {
            Transform target = m_attachToPlayer ? GameManager.PlayerController.transform : transform;
            if (m_clipName != string.Empty)
                SFXController.Instance.PlaySoundClip(m_collectionName, m_clipName, target);
            else
                SFXController.Instance.PlayRandomSoundClip(m_collectionName, target);
        }

        public void StopSounds()
        {
            SoundClipObject[] sounds = GetComponentsInChildren<SoundClipObject>();
            for (int s = 0; s < sounds.Length; s++)
            {
                Destroy(sounds[s].gameObject);
            }
        }
    }
}
