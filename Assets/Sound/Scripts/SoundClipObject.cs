///<summary>
/// Author: Halen
///
/// Object for instantiating sounds in the scene.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    public class SoundClipObject : MonoBehaviour
    {
        private AudioSource m_source;
        private bool m_isPlaying = false;

        public void Init(SFXController.SoundClip soundClip)
        {
            // get source component
            m_source = GetComponent<AudioSource>();
            m_source.clip = soundClip.audioClip;
            m_source.playOnAwake = true;

            // set variables
            m_source.volume = soundClip.volume;
            m_source.maxDistance = soundClip.range;
            m_source.loop = soundClip.loop;

            // play clip
            m_source.Play();
            m_isPlaying = true;
        }

        public void Update()
        {
            // don't destroy the object if it should loop
            if (m_source.loop)
                return;
                
            // destroy SoundClipObject when clip finishes playing
            if (m_isPlaying && !m_source.isPlaying)
                Destroy(gameObject);
        }
    }

}
