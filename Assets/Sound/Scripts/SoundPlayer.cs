///<summary>
/// Author: Halen
///
/// Plays a sound on start.
///
///</summary>

using System.Collections;
using UnityEngine;

namespace Millivolt.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] string m_collectionName;
        [SerializeField] string m_clipName;

        [SerializeField] private bool m_playOnStart = false;
        [SerializeField] private bool m_attachToPlayer = false;

        [SerializeField] private SoundType m_type;
        
        private void Start()
        {
            if (m_playOnStart)
                StartCoroutine(StartSound());
        }

        private IEnumerator StartSound()
        {
            yield return new WaitUntil(() => SFXController.Instance != null);
            yield return null;
            print(name + " Played a sound");
            PlaySound();
        }

        public void PlaySound()
        {
            if (!SFXController.Instance)
                return;

            Transform target = m_attachToPlayer ? GameManager.Player.Controller.transform : transform;
            if (m_clipName != string.Empty)
                SFXController.Instance.PlaySoundClip(m_collectionName, m_clipName, target, m_type);
            else
                SFXController.Instance.PlayRandomSoundClip(m_collectionName, target, m_type);
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
